using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Rooms.Data;
using DumbProject.Rooms.Cells;
using DG.Tweening;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    /// <summary>
    /// Classe astatta padre di ogni tipo di stanza
    /// </summary>
    public abstract class Room : MonoBehaviour
    {
        [HideInInspector]
        public List<Cell> CellsInRoom;
        [HideInInspector]
        public List<GameObject> DoorsInRoom = new List<GameObject>();
        [HideInInspector]
        public RoomMovement RoomMovment;
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
            TrimCellWalls();
            PlaceDoors();
            TrimCellPillars();
        }
        
        /// <summary>
        /// Funzione che contiene le azioni da eseguire quando la posizione in cui si vuole metere la stanza è valida e la stanza viene piazzata
        /// </summary>
        public void PlaceAction()
        {
            foreach (Cell cell in CellsInRoom)
            {
                cell.SetRelativeNode(cell.GetMyPositionOnGrid(GameManager.I.MainGridCtrl));
            }
            transform.parent = null;
            Destroy(RoomMovment);
        }

        /// <summary>
        /// Funzione che controlla che la posizione di ogni cella della stanza sia valida
        /// </summary>
        public bool CheckPosition()
        {
            bool IsValidPosition = false;
            foreach (Cell cell in CellsInRoom)
            {
                GridNode node;
                if (!cell.CheckValidPosition(GameManager.I.MainGridCtrl, out node))
                {
                    return false;
                }
                foreach (GridNode adjacentNode in node.AdjacentNodes)
                {
                    if (adjacentNode.RelativeCell != null)
                        IsValidPosition = true;
                }
            }

            return IsValidPosition;
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
                    {
                        cell.ResetRelativeNode();
                    }
                });
            }
        }

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

        /// <summary>
        /// Funzione che piazza la stanza sulla griglia nella UI
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_grid"></param>
        protected abstract void PlaceCells(RoomData _data, GridController _grid);
        
        /// <summary>
        /// Funzione che rimuove i muri se sono nella stessa posizione
        /// </summary>
        void TrimCellWalls()
        {
            List<GameObject> itemsToBeDestroyed = new List<GameObject>();
            List<GameObject> listOfWalls = GetListOfWalls();

            foreach (GameObject wall1 in listOfWalls)
            {
                foreach (GameObject wall2 in listOfWalls)
                {
                    if(wall1 != wall2 && !itemsToBeDestroyed.Contains(wall1) && !itemsToBeDestroyed.Contains(wall2))
                    {
                        if(Vector3.Distance(wall1.transform.position, wall2.transform.position) <= Data.WallPenetrationOffset)
                        {
                            itemsToBeDestroyed.Add(wall1);
                            itemsToBeDestroyed.Add(wall2);
                        }
                    }
                }
            }

            foreach (Cell cell in CellsInRoom)
            {
                List<GameObject> list = cell.GetEdgesList();
                foreach (GameObject wallToRemove in itemsToBeDestroyed)
                {
                    if (list.Contains(wallToRemove))
                    {
                        list.Remove(wallToRemove);
                        Destroy(wallToRemove);
                    }
                }
            }
        }

        /// <summary>
        /// Funzione che rimuove i pilastri se sono nella stessa posizione
        /// </summary>
        void TrimCellPillars()
        {
            List<GameObject> itemsToBeDestroyed = new List<GameObject>();
            List<GameObject> listOfPillars = GetListOfPillars();

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
        /// Funzione che sostiuisce a random alcuni muri con delle porte
        /// </summary>
        void PlaceDoors()
        {
            int numberOfDoors = Random.Range(1, CellsInRoom.Count + 1);
            List<GameObject> listOfWalls = GetListOfWalls();
            while (numberOfDoors > 0)
            {
                int randomIndex = Random.Range(0, listOfWalls.Count);
                GameObject newObj = null;
                Cell parentCell = null;

                Destroy(listOfWalls[randomIndex].GetComponentInChildren<MeshRenderer>().gameObject);
                parentCell = listOfWalls[randomIndex].transform.parent.GetComponent<Cell>();

                if (listOfWalls[randomIndex].name == "RightEdge" || listOfWalls[randomIndex].name == "LeftEdge")
                {            
                    newObj = Instantiate(Data.RoomElements.ArchPrefab, listOfWalls[randomIndex].transform.position, Quaternion.identity, listOfWalls[randomIndex].transform);
                }
                else if (listOfWalls[randomIndex].name == "UpEdge")
                {
                    newObj = Instantiate(Data.RoomElements.ArchPrefab, listOfWalls[randomIndex].transform.position, Quaternion.LookRotation(parentCell.GetAnglesList().Find(a => a.name == "NE_Angle").transform.position - listOfWalls[randomIndex].transform.position), listOfWalls[randomIndex].transform);
                }
                else if (listOfWalls[randomIndex].name == "DownEdge")
                {
                    newObj = Instantiate(Data.RoomElements.ArchPrefab, listOfWalls[randomIndex].transform.position, Quaternion.LookRotation(parentCell.GetAnglesList().Find(a => a.name == "SE_Angle").transform.position - listOfWalls[randomIndex].transform.position), listOfWalls[randomIndex].transform);
                }

                newObj.tag = "Door";
                DoorsInRoom.Add(listOfWalls[randomIndex]);
                listOfWalls.Remove(listOfWalls[randomIndex]);
                numberOfDoors--;
            }
        }

        /// <summary>
        /// Ritorna la lista dei muri contenuti in tutte le celle
        /// </summary>
        /// <returns></returns>
        List<GameObject> GetListOfWalls()
        {
            List<GameObject> listOfWalls = new List<GameObject>();
            foreach (Cell cell in CellsInRoom)
            {
                foreach (GameObject wall in cell.GetEdgesList())
                {
                    listOfWalls.Add(wall);
                }
            }
            return listOfWalls;
        }

        /// <summary>
        /// Ritorna la lista dei pilastri contenuti in tutte le celle
        /// </summary>
        /// <returns></returns>
        List<GameObject> GetListOfPillars()
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
    }                                      
                                           
    public enum RoomShape
    {
        T_Shape = 0,
        I_Shape = 1,
        J_Shape = 2,
        L_Shape = 3,
        S_Shape = 4,
        Z_Shape = 5,
        O_Shape = 6,
    }
}