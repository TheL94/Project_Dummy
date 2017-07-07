using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using DumbProject.Grid;
using DumbProject.Generic;
using DumbProject.Items;
using System;

namespace DumbProject.Rooms
{
    /// <summary>
    /// Classe astatta padre di ogni tipo di stanza
    /// </summary>
    public class Room : MonoBehaviour, IDroppableHolder
    {
        ExplorationStatus _status = ExplorationStatus.NotInGame;
        public ExplorationStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                GameManager.I.DungeonMng.SetRoomStausBasedOnCloseRoomsStatus(this);
            }
        }

        [HideInInspector]
        public List<Cell> CellsInRoom = new List<Cell>();
        [HideInInspector]
        public RoomMovement RoomMovment;
        [HideInInspector]
        public DropController DropController;
        [HideInInspector]
        public RoomData Data;

        List<Cell> _freeCells = new List<Cell>();
        List<Cell> freeCells
        {
            get
            {
                _freeCells = CellsInRoom.Where(c => c.ActualDroppable == null).ToList();
                return _freeCells;
            }
        }

        Vector3 _initialPosition;
        Tweener rotationTween;
        public Vector3 InitialPosition
        {
            get { return _initialPosition; }
            private set { _initialPosition = value; }
        }

        bool canRotate = true;
        float cellProbability = 1f;

        #region API
        /// <summary>
        /// Setup della room nella UI
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_grid"></param>
        /// <param name="_roomMovment"></param>
        public void Setup(RoomData _data, GridController _grid, RoomMovement _roomMovment)
        {
            RoomMovment = _roomMovment;
            RoomMovment.Init(this);
            InitialPosition = transform.position;
            Setup(_data, _grid);
        }

        /// <summary>
        /// Setup della room come main room
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_grid"></param>
        public void Setup(RoomData _data, GridController _grid)
        {
            Data = _data;
            PlaceCells(_data, _grid);
            TrimCellEdges(_grid);
            PlaceDoors(_data);
            TrimCellAngles();
        }

        /// <summary>
        /// Funzione che contiene le azioni da eseguire quando la posizione in cui si vuole metere la stanza è valida e la stanza viene piazzata
        /// </summary>
        public void PlaceAction()
        {
            foreach (Cell cell in CellsInRoom)
                cell.SetRelativeNode(GameManager.I.MainGridCtrl.GetSpecificGridNode(cell.transform.position));

            GameManager.I.DungeonMng.ParentRoom(this);
            TrimCollidingEdges(GameManager.I.MainGridCtrl);
            Destroy(RoomMovment);
        }

        /// <summary>
        /// funzione che fa ruotare la stanza attorno al suo centro in senso orario sulla griglia 
        /// </summary>
        public void RotateClockwise()
        {
            if (canRotate)
            {
                canRotate = false;
                rotationTween = transform.DORotate(transform.up * 90f, 0.5f, RotateMode.LocalAxisAdd).OnComplete(() =>
                {
                    canRotate = true;
                    foreach (Cell cell in CellsInRoom)
                        cell.SetRelativeNode();
                });
            }
        }

        /// <summary>
        /// Funzione che collega tra di loro le stanze
        /// </summary>
        public void LinkCells()
        {
            foreach (Cell cell in CellsInRoom)
                cell.UpdateRelativeNodeLinks();
        }

        /// <summary>
        /// Ritorna la lista dei muri contenuti in tutte le celle
        /// </summary>
        /// <returns></returns>
        public List<Edge> GetListOfEdges()
        {
            List<Edge> listOfEdges = new List<Edge>();
            foreach (Cell cell in CellsInRoom)
            {
                foreach (Edge edge in cell.GetEdgesList())
                {
                    listOfEdges.Add(edge);
                }
            }
            return listOfEdges;
        }

        #region IDroppableHolder
        List<IDroppable> _droppableList = new List<IDroppable>();
        public List<IDroppable> DroppableList { get { return _droppableList; } set { _droppableList = value; } }
        public IDroppable AddDroppable(DroppableBaseData _droppableToAdd)
        {
            Cell freeCell = freeCells[UnityEngine.Random.Range(0, freeCells.Count)];
            IDroppable dropToAdd = Instantiate(_droppableToAdd.ItemPrefab, freeCell.RelativeNode.WorldPosition, Quaternion.identity, transform).GetComponent<IDroppable>();
            freeCell.ChangeFloorColor(_droppableToAdd.ShowMateriaInRoom);
            freeCell.ActualDroppable = dropToAdd;
            DroppableList.Add(dropToAdd);
            return dropToAdd;
        }

        public void RemoveDroppable(IDroppable _droppableToRemove)
        {
            Cell cell = null;
            foreach (Cell _cell in CellsInRoom)
            {
                if (_cell.ActualDroppable == _droppableToRemove)
                {
                    cell = _cell;
                    break;
                }
            }
            if(cell != null)
            {
                cell.ActualDroppable = null;
                //_______
                //TODO: ricolorare pavimento?
                //__________
            }

            DroppableList.Remove(_droppableToRemove);
        }
        #endregion
        #endregion

        #region Cell Managment
        /// <summary>
        /// Funzione che piazza la stanza sulla griglia nella UI
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_grid"></param>
        void PlaceCells(RoomData _data, GridController _grid)
        {
            while (EvaluateCellProbability(_data, _grid))
            {
                CellsInRoom.Add(PlaceSingleCell(_data, EvaluateGridPosition(_grid)));
            }
        }

        /// <summary>
        /// Funzione che crea e piazza una cella
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_node"></param>
        /// <returns></returns>
        Cell PlaceSingleCell(RoomData _data, GridNode _node)
        {
            GameObject newCellObj = new GameObject("Cell_" + CellsInRoom.Count);
            Cell newCell = newCellObj.AddComponent<Cell>();
            newCell.PlaceCell(_node, this);
            if (_data.SelfTrimming)
                cellProbability -= 1f / 9f;
            else
                cellProbability -= _data.RoomExpansionPercentageDecay;
            return newCell;
        }

        /// <summary>
        /// Funzione che ritorna un nodo vicino a una cella appena piazzata
        /// </summary>
        /// <param name="_grid"></param>
        /// <returns></returns>
        GridNode EvaluateGridPosition(GridController _grid)
        {
            GridNode nodeToReturn = null;
            GridNode centerNode = _grid.GetGridCenter();
            if (CellsInRoom.Count == 0)
                nodeToReturn = centerNode;
            else
            {
                List<GridNode> adjacentNodes = GetEmptyGridNodes();
                if (adjacentNodes.Count > 0)
                    nodeToReturn = adjacentNodes[UnityEngine.Random.Range(0, adjacentNodes.Count)];
            }
            return nodeToReturn;
        }

        /// <summary>
        /// Funzione che ritorna una lista di nodi liberi
        /// </summary>
        /// <returns></returns>
        List<GridNode> GetEmptyGridNodes()
        {
            List<GridNode> adjacentNodes = new List<GridNode>();
            foreach (Cell cell in CellsInRoom)
            {
                adjacentNodes.AddRange(cell.RelativeNode.AdjacentNodes.Where(a => a.RelativeCell == null).ToList());
            }
            return adjacentNodes;
        }

        /// <summary>
        /// Funzione che valuta la probabiltà di piazzare una cella aggiuntiva
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_grid"></param>
        /// <returns></returns>
        bool EvaluateCellProbability(RoomData _data, GridController _grid)
        {
            if (_data.MinNumberOfCells > CellsInRoom.Count)
                return true;

            float randomProbabaility = UnityEngine.Random.Range(0f, 1f);
            if (cellProbability >= randomProbabaility)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Funzione che rimuove i muri se sono nella stessa posizione
        /// </summary>
        void TrimCellEdges(GridController _grid)
        {
            List<Edge> itemsToBeDestroyed = new List<Edge>();
            List<Edge> edges = GetListOfEdges();
            foreach (Edge edge in edges)
                edge.CheckCollisionWithOtherEdges(_grid);

            foreach (Edge edge in edges)
            {
                if (!itemsToBeDestroyed.Contains(edge))
                {
                    if (edge.CollidingEdge != null && edge.CollidingEdge.Type == EdgeType.Wall)
                    {
                        itemsToBeDestroyed.Add(edge);
                    }
                }
            }

            foreach (Cell cell in CellsInRoom)
            {
                List<Edge> list = cell.GetEdgesList();
                foreach (Edge wallToRemove in itemsToBeDestroyed)
                {
                    if (list.Contains(wallToRemove))
                    {
                        list.Remove(wallToRemove);
                        wallToRemove.gameObject.SetActive(false);
                    }
                }
            }
        }

        /// <summary>
        /// Funzione che rimuove i pilastri se sono nella stessa posizione
        /// </summary>
        void TrimCellAngles()
        {
            List<GameObject> itemsToBeDestroyed = new List<GameObject>();
            List<GameObject> listOfPillars = GetListOfAngles();

            foreach (GameObject pillar1 in listOfPillars)
            {
                foreach (GameObject pillar2 in listOfPillars)
                {
                    if (pillar1 != pillar2 && !itemsToBeDestroyed.Contains(pillar1) && !itemsToBeDestroyed.Contains(pillar2))
                    {
                        if (pillar1.transform.position == pillar2.transform.position)
                        {
                            itemsToBeDestroyed.Add(pillar2);
                        }
                    }
                }
            }

            foreach (Cell cell in CellsInRoom)
            {
                List<GameObject> list = cell.GetAnglesList();
                foreach (GameObject pillarToRemove in itemsToBeDestroyed)
                {
                    if (list.Contains(pillarToRemove))
                    {
                        list.Remove(pillarToRemove);
                        pillarToRemove.SetActive(false);
                    }
                }
            }
        }

        /// <summary>
        /// Funzione che distrugge 
        /// </summary>
        /// <param name="_grid"></param>
        void TrimCollidingEdges(GridController _grid)
        {
            List<Edge> itemsToBeDestroyed = new List<Edge>();
            List<Edge> edges = GetListOfEdges();

            foreach (Edge edge in edges)
            {
                if (!itemsToBeDestroyed.Contains(edge.CollidingEdge))
                {
                    if (edge.Type == EdgeType.Door && edge.CollidingEdge != null)
                    {
                        itemsToBeDestroyed.Add(edge.CollidingEdge);
                    }
                    else if (edge.Type == EdgeType.Wall && edge.CollidingEdge != null && edge.CollidingEdge.Type == EdgeType.Door)
                    {
                        itemsToBeDestroyed.Add(edge);
                    }

                    //###################################
                    // CONTROLLO DA NON CANCELLARE !
                    // serve per distruggere i muri che collidono fra di loro quando piazzo la stanza, da usare o meno in funzione della grafica.
                    //else if (edge.Type == EdgeType.Wall && edge.CollidingEdge != null && edge.CollidingEdge.Type == EdgeType.Wall)
                    //{
                    //    itemsToBeDestroyed.Add(edge);
                    //}
                    //###################################
                }
            }

            foreach (Edge egdeToDestroy in itemsToBeDestroyed)
            {
                if (egdeToDestroy.RelativeCell.GetEdgesList().Contains(egdeToDestroy))
                {
                    egdeToDestroy.RelativeCell.GetEdgesList().Remove(egdeToDestroy);
                    egdeToDestroy.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Funzione che sostiuisce a random alcuni muri con delle porte
        /// </summary>
        void PlaceDoors(RoomData _data)
        {
            float maxAmountOfDoors = _data.SelfDoorTrim ? CellsInRoom.Count + 1 : _data.DoorNumberPercentage * CellsInRoom.Count;
            if (maxAmountOfDoors < 1)
                maxAmountOfDoors = 1;
            int numberOfDoors = Mathf.RoundToInt(UnityEngine.Random.Range(1f, maxAmountOfDoors));

            List<Edge> listOfEdges = GetListOfEdges();
            while (numberOfDoors > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, listOfEdges.Count);
                if (ReplaceWallWithArch(listOfEdges[randomIndex]))
                    numberOfDoors--;
            }
        }

        /// <summary>
        /// Funzione che sostituisce la grafica del muro con la grafica porta
        /// </summary>
        /// <param name="_edge"></param>
        bool ReplaceWallWithArch(Edge _edge)
        {
            if (_edge.Type == EdgeType.Door)
                return false;

            Cell parentCell = null;
            _edge.GetComponentInChildren<MeshRenderer>().gameObject.SetActive(false);
            parentCell = _edge.transform.parent.GetComponent<Cell>();

            if (_edge.name == "RightEdge")
            {
                _edge.SetGraphic(GameManager.I.PoolMng.GetGameObject(ObjType.Arch), Quaternion.identity);
                _edge.name = "RightDoor";
            }
            else if (_edge.name == "LeftEdge")
            {
                _edge.SetGraphic(GameManager.I.PoolMng.GetGameObject(ObjType.Arch), Quaternion.identity);
                _edge.name = "LeftDoor";
            }
            else if (_edge.name == "UpEdge")
            {
                _edge.SetGraphic(GameManager.I.PoolMng.GetGameObject(ObjType.Arch),
                    Quaternion.LookRotation(parentCell.GetAnglesList().Find(a => a.name == "NE_Angle").transform.position - _edge.transform.position));
                _edge.name = "UpDoor";
            }
            else if (_edge.name == "DownEdge")
            {
                _edge.SetGraphic(GameManager.I.PoolMng.GetGameObject(ObjType.Arch),
                    Quaternion.LookRotation(parentCell.GetAnglesList().Find(a => a.name == "SE_Angle").transform.position - _edge.transform.position));
                _edge.name = "DownDoor";
            }
            _edge.Type = EdgeType.Door;
            return true;
        }

        /// <summary>
        /// Ritorna la lista dei pilastri contenuti in tutte le celle
        /// </summary>
        /// <returns></returns>
        List<GameObject> GetListOfAngles()
        {
            List<GameObject> listOfPillars = new List<GameObject>();
            foreach (Cell cell in CellsInRoom)
            {
                foreach (GameObject wall in cell.GetAnglesList())
                {
                    listOfPillars.Add(wall);
                }
            }
            return listOfPillars;
        }
        #endregion
    }

    public enum ExplorationStatus { NotInGame = -1, Unavailable = 0, Unexplored = 1, Explored = 2 }
}