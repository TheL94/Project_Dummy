using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DumbProject.Grid;
using DumbProject.Generic;
using DumbProject.Items;

namespace DumbProject.Rooms
{
    /// <summary>
    /// Classe astatta padre di ogni tipo di stanza
    /// </summary>
    public class Room : MonoBehaviour, IInteractableHolder
    {
        ExplorationStatus _statusOfExploration = ExplorationStatus.NotInGame;
        public ExplorationStatus StatusOfExploration
        {
            get
            {
                return _statusOfExploration;
            }
            set
            {
                _statusOfExploration = value;
            }
        }

        protected List<Door> doors = new List<Door>();

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

            TrimCollidingEdges(GameManager.I.MainGridCtrl);
            GameManager.I.DungeonMng.ParentRoom(this);

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

        /// <summary>
        /// Ritorna la lista delle porte contenute in tutte le celle
        /// </summary>
        /// <returns></returns>
        public List<Door> GetListOfDoors()
        {
            //List<Door> listOfDoors = new List<Door>();
            //foreach (Cell cell in CellsInRoom)
            //{
            //    foreach (Door door in cell.Doors)
            //    {
            //        listOfDoors.Add(door);
            //    }
            //}
            return doors;
        }

        public void SetDoors(List<Door> _doors)
        {
            foreach (Door newDoor in _doors)
            {
                if (!doors.Contains(newDoor))
                    doors.Add(newDoor);
            }
        }

        #region IDroppableHolder
        List<IInteractable> _interactableList = new List<IInteractable>();
        /// <summary>
        /// Do not ADD on this property. Use AddInteractable function instead
        /// </summary>
        public List<IInteractable> InteractableAvailable
        {
            get { return _interactableList.Where(a => a.IsInteractable == true).ToList(); }
            set { _interactableList = value; }
        }
        public List<IInteractable> InteractableList
        {
            get { return _interactableList; }
            set { _interactableList = value; }
        }
        public IInteractable AddInteractable(IDroppable _droppableToAdd)
        {
            List<Cell> freeCells = CellsInRoom.Where(c => c.ActualInteractable == null).ToList();
            Cell freeCell = freeCells[Random.Range(0, freeCells.Count)];
            freeCell.ActualInteractable = _droppableToAdd;
            freeCell.ChangeFloorColor(_droppableToAdd.Data.ShowMateriaInRoom);
            InteractableList.Add(_droppableToAdd);
            return _droppableToAdd;
        }

        public void RemoveInteractable(IInteractable _interactableToRemove)
        {
            Cell cell = null;
            foreach (Cell _cell in CellsInRoom)
            {
                if (_cell.ActualInteractable == _interactableToRemove)
                {
                    cell = _cell;
                    break;
                }
            }
            if(cell != null)
            {
                cell.ActualInteractable = null;
                //_______
                //TODO: ricolorare pavimento?
                //__________
            }

            InteractableList.Remove(_interactableToRemove);
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
                    if (edge.CollidingEdge != null)
                    {
                        itemsToBeDestroyed.Add(edge);
                    }
                }
            }

            foreach (Edge wallToRemove in itemsToBeDestroyed)
            {
                wallToRemove.DisableEdge();
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
        /// Funzione che distrugge gli edge in collisione con una porta
        /// </summary>
        /// <param name="_grid"></param>
        void TrimCollidingEdges(GridController _grid)
        {
            List<Edge> itemsToBeDestroyed = new List<Edge>();
            List<Edge> edges = GetListOfEdges();

            foreach (Edge edge in edges)
            {
                if (!itemsToBeDestroyed.Contains(edge.CollidingEdge) && !itemsToBeDestroyed.Contains(edge) && edge.CollidingEdge != null)
                {
                    // porta collide con una porta o un muro
                    if (edge.GetType() == typeof(Door))
                    {
                        (edge as Door).AddAjdacentCell(edge.CollidingEdge.RelativeCell);
                        edge.CollidingEdge.RelativeCell.Doors.Add((edge as Door));
                        itemsToBeDestroyed.Add(edge.CollidingEdge);
                    }

                    // muro collide con porta
                    else if (edge.CollidingEdge.GetType() == typeof(Door))
                    {
                        (edge.CollidingEdge as Door).AddAjdacentCell(edge.RelativeCell);
                        edge.RelativeCell.Doors.Add(edge.CollidingEdge as Door);
                        itemsToBeDestroyed.Add(edge);
                    }
                }
            }

            foreach (Edge egdeToDestroy in itemsToBeDestroyed)
            {
                egdeToDestroy.DisableEdge();
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
            int numberOfDoors = Mathf.RoundToInt(Random.Range(1f, maxAmountOfDoors));

            List<Edge> listOfEdges = GetListOfEdges();
            while (numberOfDoors > 0)
            {
                int randomIndex = Random.Range(0, listOfEdges.Count);
                Edge edgeToReplace = listOfEdges[randomIndex];
                Cell relativeCell = edgeToReplace.RelativeCell;
                if (relativeCell.ReplaceEdgeWithDoor(edgeToReplace))
                    numberOfDoors--;
            }

            foreach (Cell cell in CellsInRoom)
                SetDoors(cell.Doors);
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

        private void OnDrawGizmos()
        {
            switch (StatusOfExploration)
            {
                case ExplorationStatus.NotInGame:
                    Gizmos.color = Color.white;
                    break;
                case ExplorationStatus.Unavailable:
                    Gizmos.color = Color.red;
                    break;
                case ExplorationStatus.NotExplored:
                    Gizmos.color = Color.yellow;
                    break;
                case ExplorationStatus.InExploration:
                    Gizmos.color = Color.cyan;
                    break;
                case ExplorationStatus.Explored:
                    Gizmos.color = Color.green;
                    break;
                default:
                    break;
            }
            Gizmos.DrawWireCube(transform.position + new Vector3(0f, 6f, 0f), new Vector3(5f, 1f, 5f));
        }
    }

    public enum ExplorationStatus { NotInGame = -1, Unavailable = 0, NotExplored = 1, InExploration = 2, Explored = 3 }
}