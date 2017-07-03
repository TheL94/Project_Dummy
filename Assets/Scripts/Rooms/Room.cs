using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using DumbProject.Grid;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    /// <summary>
    /// Classe astatta padre di ogni tipo di stanza
    /// </summary>
    public class Room : MonoBehaviour
    {
        [HideInInspector]
        public List<Cell> CellsInRoom = new List<Cell>();
        [HideInInspector]
        public RoomMovement RoomMovment;
        [HideInInspector]
        public DropController DropController;
        [HideInInspector]
        public RoomData Data;

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
            LinkCellsInsideRoom();
            LinkCellsDoorsToFallingPoints();
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
            LinkCellsToOtherRooms();
            LinkCellsDoorsToFallingPoints();
            GameManager.I.DungeonMng.UpdateRoomConnections();
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
        /// Funzione che collaga fra di loro le celle adiacenti di stanze diverse
        /// </summary>
        public void LinkCellsToOtherRooms()
        {
            foreach (Cell cell in CellsInRoom)
                cell.LinkCellToOtherRoomsCells();
        }

        /// <summary>
        /// Funzione che collega celle e punti di caduta
        /// </summary>
        public void LinkCellsDoorsToFallingPoints()
        {
            foreach (Cell cell in CellsInRoom)
                cell.LinkDoorsToFallingPoint();
        }

        #region Object Placement
        /// <summary>
        /// Ritorna una cella random dove istanziare l'item, se la cella è occupata, ne cerca una libera.
        /// </summary>
        /// <returns>cella libera</returns>
        public Cell ChooseFreeCell()
        {
            int tempNum = Random.Range(0, CellsInRoom.Count);
            if (CellsInRoom[tempNum].IsFree)
                return CellsInRoom[tempNum];
            else
            {
                for (int i = 0; i < CellsInRoom.Count; i++)
                {
                    if (CellsInRoom[i].IsFree)
                        return CellsInRoom[i];
                }
            }
            return null;
        }
        #endregion
        #endregion

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
                    nodeToReturn = adjacentNodes[Random.Range(0, adjacentNodes.Count)];
            }
            return nodeToReturn;
        }

        List<GridNode> GetEmptyGridNodes()
        {
            List<GridNode> adjacentNodes = new List<GridNode>();
            foreach (Cell cell in CellsInRoom)
            {
                adjacentNodes.AddRange(cell.RelativeNode.AdjacentNodes.Where(a => a.RelativeCell == null).ToList());
            }
            return adjacentNodes;
        }

        bool EvaluateCellProbability(RoomData _data, GridController _grid)
        {
            if (_data.MinNumberOfCells > CellsInRoom.Count)
                return true;

            float randomProbabaility = Random.Range(0f, 1f);
            if (cellProbability >= randomProbabaility)
                return true;
            else
                return false;
        }

        #region Cell Managment
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
                if(!itemsToBeDestroyed.Contains(edge))
                {
                    if(edge.CollidingEdge != null && edge.CollidingEdge.Type == EdgeType.Wall)
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
                        Destroy(wallToRemove.gameObject);
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
                        Destroy(pillarToRemove);
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
                    Destroy(egdeToDestroy.gameObject);
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
            int numberOfDoors = Mathf.RoundToInt(Random.Range(1f,maxAmountOfDoors));

            List<Edge> listOfWalls = GetListOfEdges();
            while (numberOfDoors > 0)
            {
                int randomIndex = Random.Range(0, listOfWalls.Count);
                if(ReplaceWallWithArch(listOfWalls[randomIndex]))
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

            GameObject newObj = null;
            Cell parentCell = null;
            Destroy(_edge.GetComponentInChildren<MeshRenderer>().gameObject);
            parentCell = _edge.transform.parent.GetComponent<Cell>();
            if (_edge.name == "RightEdge")
            {
                newObj = Instantiate(Data.RoomElements.ArchPrefab, _edge.transform.position, Quaternion.identity, _edge.transform);
                _edge.Type = EdgeType.Door;
                _edge.name = "RightDoor";
            }
            else if (_edge.name == "LeftEdge")
            {
                newObj = Instantiate(Data.RoomElements.ArchPrefab, _edge.transform.position, Quaternion.identity, _edge.transform);
                _edge.Type = EdgeType.Door;
                _edge.name = "LeftDoor";
            }
            else if (_edge.name == "UpEdge")
            {
                newObj = Instantiate(Data.RoomElements.ArchPrefab, _edge.transform.position, 
                    Quaternion.LookRotation(parentCell.GetAnglesList().Find(a => a.name == "NE_Angle").transform.position - _edge.transform.position), _edge.transform);
                _edge.Type = EdgeType.Door;
                _edge.name = "UpDoor";
            }
            else if (_edge.name == "DownEdge")
            {
                newObj = Instantiate(Data.RoomElements.ArchPrefab, _edge.transform.position, 
                    Quaternion.LookRotation(parentCell.GetAnglesList().Find(a => a.name == "SE_Angle").transform.position - _edge.transform.position), _edge.transform);
                _edge.Type = EdgeType.Door;
                _edge.name = "DownDoor";
            }
            return true;
        }

        /// <summary>
        /// Funzione che collaga fra di loro le celle adiacenti della stessa stanza
        /// </summary>
        void LinkCellsInsideRoom()
        {
            foreach (Cell cell in CellsInRoom)
                cell.LinkCellToRelativeRoomCells();
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

        /// <summary>
        /// Ritorna la lista dei muri contenuti in tutte le celle
        /// </summary>
        /// <returns></returns>
        List<Edge> GetListOfEdges()
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
        #endregion
    }                                      
}