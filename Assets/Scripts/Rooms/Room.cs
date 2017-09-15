
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
            get { return _statusOfExploration; }
            set { _statusOfExploration = value; }
        }

        [HideInInspector]
        public List<Cell> CellsInRoom = new List<Cell>();
        [HideInInspector]
        public RoomMovement RoomMovment;
        [HideInInspector]
        public DropController DropController;
        [HideInInspector]
        public RoomData Data;

        float cellProbability = 1f;

        #region API
        /// <summary>
        /// Setup della room nella UI
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_grid"></param>
        /// <param name="_roomMovment"></param>
        public void Setup(RoomData _data, GridNode _centerNode, RoomMovement _roomMovment)
        {
            RoomMovment = _roomMovment;
            Setup(_data, _centerNode);
        }

        /// <summary>
        /// Setup della room come main room
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_grid"></param>
        public void Setup(RoomData _data, GridNode _centerNode)
        {
            transform.position = _centerNode.WorldPosition;
            Data = _data;
            PlaceCells(_data, _centerNode);
            TrimCellEdges(_centerNode.RelativeGrid);
            PlaceDoors(_data);
            TrimCellAngles();
            PlaceOtherGraphicElements();
        }

        /// <summary>
        /// Funzione che ritorna tutti gli oggetti grafici al pool prima di distruggere la stanza e le relative celle
        /// </summary>
        public void DestroyChildrenObject()
        {
            for (int i = 0; i < CellsInRoom.Count; i++)
            {
                CellsInRoom[i].Floor.DisableAllGraphics();

                for (int j = 0; j < CellsInRoom[i].Angles.Count; j++)
                    CellsInRoom[i].Angles[j].DisableAllGraphics();

                for (int j = 0; j < CellsInRoom[i].Edges.Count; j++)
                    CellsInRoom[i].Edges[j].DisableAllGraphics();

                for (int j = 0; j < CellsInRoom[i].Doors.Count; j++)
                    CellsInRoom[i].Doors[j].DisableAllGraphics();
            }

            for (int i = 0; i < CellsInRoom.Count; i++)
            {
                CellsInRoom[i].Floor.DestroyObject();

                for (int j = 0; j < CellsInRoom[i].Angles.Count; j++)
                    CellsInRoom[i].Angles[j].DestroyObject();

                for (int j = 0; j < CellsInRoom[i].Edges.Count; j++)
                    CellsInRoom[i].Edges[j].DestroyObject();

                for (int j = 0; j < CellsInRoom[i].Doors.Count; j++)
                    CellsInRoom[i].Doors[j].DestroyObject();
            }
        }

        /// <summary>
        /// Funzione che contiene le azioni da eseguire quando la posizione in cui si vuole metere la stanza è valida e la stanza viene piazzata
        /// </summary>
        public void PlaceAction()
        {
            foreach (Cell cell in CellsInRoom)
                cell.RelativeNode = GameManager.I.MainGridCtrl.GetSpecificGridNode(cell.transform.position);

            AddNetNodesOnDoors();
            TrimCollidingEdges(GameManager.I.MainGridCtrl);
            TrimCollidingAngles();
            GameManager.I.DungeonMng.SetupRoomInDungeon(this);

            Destroy(RoomMovment);
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
        /// Funzione che aggiunge un net node nella posizione della porta
        /// </summary>
        public void AddNetNodesOnDoors()
        {
            GridController relativeGrid = CellsInRoom[0].RelativeNode.RelativeGrid;
            List<GenericNode> adjacentNodes;
            foreach (Door door in GetDoors())
            {
                adjacentNodes = new List<GenericNode>() { door.RelativeCell.RelativeNode as GenericNode,
                    relativeGrid.GetSpecificGridNode(door.GetOppositeOfRelativeCellPosition()) };
                door.RelativeNetNode = relativeGrid.AddNewNetNode(door.transform.position);
                door.RelativeNetNode.Init(adjacentNodes, door);
            }
        }

        public void HighlightRoomWalls(bool _toggle, Color _color)
        {
            List<Edge> roomEdges = GetEdges();
            roomEdges.AddRange(GetDoors().ConvertAll(d => d as Edge));

            foreach (Edge edge in roomEdges)
                edge.ToggleEmissive(_toggle, _color);
        }

        /// <summary>
        /// Ritorna la lista dei muri contenuti in tutte le celle
        /// </summary>
        /// <returns></returns>
        public List<Edge> GetEdges()
        {
            List<Edge> listOfEdges = new List<Edge>();
            foreach (Cell cell in CellsInRoom)
                listOfEdges.AddRange(cell.Edges);
            return listOfEdges;
        }

        /// <summary>
        /// Ritorna la lista delle porte contenute in tutte le celle
        /// </summary>
        /// <returns></returns>
        public List<Door> GetDoors()
        {
            List<Door> doors = new List<Door>();
            foreach (Cell cell in CellsInRoom)
                doors.AddRange(cell.Doors);
            return doors;
        }

        /// <summary>
        /// Ritorna la lista dei pilastri contenuti in tutte le celle
        /// </summary>
        /// <returns></returns>
        public List<Angle> GetAngles()
        {
            List<Angle> listOfAngles = new List<Angle>();
            foreach (Cell cell in CellsInRoom)
                listOfAngles.AddRange(cell.Angles);

            return listOfAngles;
        }

        public void SetItemIndicator(bool _isInUI)
        {
            foreach (ItemIndicator indicator in GetComponentsInChildren<ItemIndicator>())
            {
                indicator.inUI = _isInUI;
            }
        }

        #region IDroppableHolder
        List<IInteractable> _interactableList = new List<IInteractable>();
        /// <summary>
        /// Do not ADD on this property. Use AddInteractable function instead
        /// </summary>
        public List<IInteractable> InteractableAvailable
        {
            get { /*return _interactableList.Where(a => a.IsInteractable == true).ToList();*/ return FilterAvailable(InteractableList); }
            set { _interactableList = value; }
        }
        public List<IInteractable> InteractableList
        {
            get { return _interactableList; }
            set { _interactableList = value; }
        }

        List<IInteractable> FilterAvailable(List<IInteractable> _list)
        {
            List<IInteractable> listToReturn = new List<IInteractable>();
            foreach (IInteractable interactable in _list)
            {
                if (interactable.IsInteractable)
                {
                    listToReturn.Add(interactable);
                }
            }
            return listToReturn;
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

        #region Cell Filler Graphic
        /// <summary>
        /// Funzione che piazza elementi di grafica riempitiva nella stanza
        /// </summary>
        public void PlaceOtherGraphicElements()
        {
            // TODO : alla distruzione causa blocco del gioco
            List<Edge> edges = GetEdges();
            List<Angle> angles = GetAngles();

            Edge edge = edges[Random.Range(0, edges.Count)];
            Vector3 position = ((edge.RelativeCell.transform.position + edge.transform.position) / 2);
            Quaternion rotation = Quaternion.LookRotation(edge.RelativeCell.transform.position - position);
            edge.SetFillerGraphic(GameManager.I.PoolMng.GetGameObject("Barrel1"), position, rotation);
            edges.Remove(edge);

            edge = edges[Random.Range(0, edges.Count)];
            position = ((edge.RelativeCell.transform.position + edge.transform.position) / 2);
            rotation = Quaternion.LookRotation(edge.RelativeCell.transform.position - position);
            edge.SetFillerGraphic(GameManager.I.PoolMng.GetGameObject("Barrel2"), position, rotation);
            edges.Remove(edge);

            edge = edges[Random.Range(0, edges.Count)];
            position = ((edge.RelativeCell.transform.position + edge.transform.position) / 2);
            rotation = Quaternion.LookRotation(edge.RelativeCell.transform.position - position);      
            edge.SetFillerGraphic(GameManager.I.PoolMng.GetGameObject("Chest"), position, rotation);
            edges.Remove(edge);

            Angle angle = angles[Random.Range(0, angles.Count)];
            position = ((angle.RelativeCell.transform.position + angle.transform.position) / 2);
            angle.SetLightingObject(GameManager.I.PoolMng.GetGameObject("YellowLight"), position);
            angles.Remove(angle);

            angle = angles[Random.Range(0, angles.Count)];
            position = ((angle.RelativeCell.transform.position + angle.transform.position) / 2);
            angle.SetLightingObject(GameManager.I.PoolMng.GetGameObject("YellowLight"), position);
            angles.Remove(angle);
        }

        /// <summary>
        /// Funzione che ritorna una lista gi colonne che non hanno nessun muro adiacente
        /// </summary>
        /// <returns></returns>
        public List<Angle> FindAnglesWithoutWall()
        {
            List<Angle> anglesToReturn = new List<Angle>();
            List<Edge> roomEdges = GetEdges();
            roomEdges.AddRange(GetDoors().ConvertAll(d => d as Edge));
            float distance;
            List<Angle> angleToCycle = GetAngles();
            foreach (Angle angle in angleToCycle)
            {
                foreach (Edge edge in roomEdges)
                {
                    distance = Vector3.Distance(edge.transform.position, angle.transform.position);
                    if (distance > GameManager.I.MainGridCtrl.CellSize/2)
                    {
                        if (!anglesToReturn.Contains(angle))
                            anglesToReturn.Add(angle);
                    }
                    else
                    {
                        anglesToReturn.Remove(angle);
                        break;
                    }
                }
            }
            return anglesToReturn;
        }

        /// <summary>
        /// Funzione che ritorna una lista di coppie di muri che formano un angolo
        /// </summary>
        /// <returns></returns>
        public List<Edge[]> FindCornerEdges()
        {
            List<Edge[]> wallCouples = new List<Edge[]>();
            float distance = Mathf.Sqrt(Mathf.Pow(GameManager.I.MainGridCtrl.CellSize / 2, 2) * 2);
            foreach (Edge edge1 in GetEdges())
            {
                foreach (Edge edge2 in GetEdges())
                {
                    if (edge1 == edge2)
                        continue;
                    if (Vector3.Distance(edge1.transform.position, edge2.transform.position) == distance)
                    {
                        wallCouples.Add(new Edge[2] { edge1, edge2 });
                        // TODO : trovare modo di escluedere gli angoli al di fuori della cella
                    }
                }
            }

            return wallCouples;
        }
        #endregion

        #region Cell Managment
        /// <summary>
        /// Funzione che piazza la stanza sulla griglia nella UI
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_grid"></param>
        void PlaceCells(RoomData _data, GridNode _centerNode)
        {
            while (EvaluateCellProbability(_data))
            {
                CellsInRoom.Add(PlaceSingleCell(_data, EvaluateGridPosition(_centerNode)));
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
        GridNode EvaluateGridPosition(GridNode _centerNode)
        {
            GridNode nodeToReturn = null;
            if (CellsInRoom.Count == 0)
                nodeToReturn = _centerNode;
            else
            {
                List<GridNode> adjacentNodes = GetEmptyGridNodes();
                if (adjacentNodes.Count > 0)
                    nodeToReturn = adjacentNodes[Random.Range(0, adjacentNodes.Count)];
            }
            return nodeToReturn;
        }

        /// <summary>
        /// Funzione che ritorna una lista di nodi liberi
        /// </summary>
        /// <returns></returns>
        List<GridNode> GetEmptyGridNodes()
        {
            List<GridNode> adjacentEmptyNodes = new List<GridNode>();
            foreach (Cell cell in CellsInRoom)
            {
                foreach (NetNode node in cell.RelativeNode.AdjacentNodes)
                {
                    if(node.GetType() == typeof(GridNode) && (node as GridNode).RelativeCell == null)
                    {
                        adjacentEmptyNodes.Add(node as GridNode);
                    }
                }
            }
            return adjacentEmptyNodes;
        }

        /// <summary>
        /// Funzione che valuta la probabiltà di piazzare una cella aggiuntiva
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_grid"></param>
        /// <returns></returns>
        bool EvaluateCellProbability(RoomData _data)
        {
            if (_data.MinNumberOfCells > CellsInRoom.Count)
                return true;

            float randomProbabaility = Random.Range(0f, 1f);
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
            List<Edge> edges = GetEdges();
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
                wallToRemove.DisableAndDestroyObject();
            }
        }

        /// <summary>
        /// Funzione che rimuove i pilastri se sono nella stessa posizione
        /// </summary>
        void TrimCellAngles()
        {
            List<Angle> itemsToBeDestroyed = new List<Angle>();
            List<Angle> listOfPillars = GetAngles();

            foreach (Angle angle1 in listOfPillars)
            {
                foreach (Angle angle2 in listOfPillars)
                {
                    if (angle1 != angle2 && !itemsToBeDestroyed.Contains(angle1) && !itemsToBeDestroyed.Contains(angle2))
                    {
                        if (angle1.transform.position == angle2.transform.position)
                        {
                            itemsToBeDestroyed.Add(angle2);
                        }
                    }
                }
            }

            foreach (Angle angle in itemsToBeDestroyed)
            {
                angle.DisableAndDestroyObject();
            }
        }

        /// <summary>
        /// Funzione che distrugge gli edge in collisione
        /// </summary>
        /// <param name="_grid"></param>
        void TrimCollidingEdges(GridController _grid)
        {
            List<Edge> itemsToBeDestroyed = new List<Edge>();
            List<Edge> roomEdges = GetEdges();
            roomEdges.AddRange(GetDoors().ConvertAll(d => d as Edge));

            foreach (Edge edge in roomEdges)
            {
                if (!itemsToBeDestroyed.Contains(edge) && edge.CollidingEdge != null && !itemsToBeDestroyed.Contains(edge.CollidingEdge))
                {
                    // porta collide con una porta o un muro
                    if (edge.GetType() == typeof(Door))
                    {
                        (edge as Door).AddAjdacentCell(edge.CollidingEdge.RelativeCell);
                        itemsToBeDestroyed.Add(edge.CollidingEdge);
                    }

                    // muro collide con porta
                    else if (edge.CollidingEdge.GetType() == typeof(Door))
                    {
                        (edge.CollidingEdge as Door).AddAjdacentCell(edge.RelativeCell);
                        itemsToBeDestroyed.Add(edge);
                    }

                    // muro collide con muro
                    else if (edge.GetType() == typeof(Edge) && edge.CollidingEdge.GetType() == typeof(Edge))
                    {
                        // cerco di distruggere il muro senza oggetti di grafica, altrimenti cambio il parent di questi ultimi
                        if (!edge.HasFillerGraphic())
                        {
                            itemsToBeDestroyed.Add(edge);
                        }
                        else
                        {
                            if (!edge.CollidingEdge.HasFillerGraphic())
                            {
                                itemsToBeDestroyed.Add(edge.CollidingEdge);
                            }
                            else
                            {
                                itemsToBeDestroyed.Add(edge);
                                edge.SwapFillerGraphicParent(edge.CollidingEdge);
                            }
                        }
                    }
                }
            }

            foreach (Edge egdeToDestroy in itemsToBeDestroyed)
            {
                egdeToDestroy.DisableAndDestroyObject();
            }
        }

        /// <summary>
        /// Funzione che distrugge gli angle in collisione
        /// </summary>
        void TrimCollidingAngles()
        {
            List<Angle> itemsToBeDestroyed = new List<Angle>();
            List<Angle> roomAngles = GetAngles();

            foreach (Angle angle in roomAngles)
            {
                angle.CheckCollisionWithOtherAngle();
                if (!itemsToBeDestroyed.Contains(angle) && angle.CollidingAngle != null && !itemsToBeDestroyed.Contains(angle.CollidingAngle))
                {
                    // cerco di distruggere la colonna senza oggetti di illuminazione, altrimenti cambio il parent di questi ultimi
                    if (!angle.HasLightingObject())
                    {
                        itemsToBeDestroyed.Add(angle);
                    }
                    else
                    {
                        if (!angle.CollidingAngle.HasLightingObject())
                        {
                            itemsToBeDestroyed.Add(angle.CollidingAngle);
                        }
                        else
                        {
                            itemsToBeDestroyed.Add(angle);
                            angle.SwapLightingObjectParent(angle.CollidingAngle);
                        }
                    }
                }
            }

            foreach (Angle angleToDestroy in itemsToBeDestroyed)
            {
                angleToDestroy.DisableAndDestroyObject();
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

            List<Edge> listOfEdges = GetEdges();
            while (numberOfDoors > 0)
            {
                int randomIndex = Random.Range(0, listOfEdges.Count);
                Edge edgeToReplace = listOfEdges[randomIndex];
                Cell relativeCell = edgeToReplace.RelativeCell;
                if (relativeCell.ReplaceEdgeWithDoor(edgeToReplace))
                {
                    numberOfDoors--;
                    listOfEdges = GetEdges();
                }
            }
        }
        #endregion

        private void OnDrawGizmos()
        {
            //switch (StatusOfExploration)
            //{
            //    case ExplorationStatus.NotInGame:
            //        Gizmos.color = Color.white;
            //        break;
            //    case ExplorationStatus.Unavailable:
            //        Gizmos.color = Color.red;
            //        break;
            //    case ExplorationStatus.NotExplored:
            //        Gizmos.color = Color.yellow;
            //        break;
            //    case ExplorationStatus.InExploration:
            //        Gizmos.color = Color.cyan;
            //        break;
            //    case ExplorationStatus.Explored:
            //        Gizmos.color = Color.green;
            //        break;
            //    default:
            //        break;
            //}
            //Gizmos.DrawWireCube(transform.position + new Vector3(0f, 4f, 0f), new Vector3(5f, 1f, 5f));
        }
    }

    public enum ExplorationStatus { NotInGame = -1, Unavailable = 0, NotExplored = 1, InExploration = 2, Explored = 3 }
}