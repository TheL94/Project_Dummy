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
            TrimCellPillars();
            UpdateCellsElements();
        }

        public void LinkRoom()
        {

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
            foreach (Cell cellInRoom in CellsInRoom)
            {
                foreach (Cell cell in CellsInRoom)
                {
                    if(cellInRoom != cell)
                    {
                        foreach (GameObject item1 in cellInRoom.Edges)
                        {
                            foreach (GameObject item2 in cell.Edges)
                            {
                                if(item1 != item2 && !itemsToBeDestroyed.Contains(item1) && !itemsToBeDestroyed.Contains(item2))
                                {
                                    if(Vector3.Distance(item1.transform.position, item2.transform.position) <= Data.WallPenetrationOffset)
                                    {
                                        itemsToBeDestroyed.Add(item1);
                                        itemsToBeDestroyed.Add(item2);
                                    }
                                }
                            }
                        }
                    }                       
                }
            }

            foreach (GameObject item in itemsToBeDestroyed)
            {
                Destroy(item);
            }
        }

        /// <summary>
        /// Funzione che rimuove i pilastri se sono nella stessa posizione
        /// </summary>
        void TrimCellPillars()
        {
            List<GameObject> itemsToBeDestroyed = new List<GameObject>();
            foreach (Cell cellInRoom in CellsInRoom)
            {
                foreach (Cell cell in CellsInRoom)
                {
                    if (cellInRoom != cell)
                    {
                        foreach (GameObject item1 in cellInRoom.Angles)
                        {
                            foreach (GameObject item2 in cell.Angles)
                            {
                                if (item1 != item2 && !itemsToBeDestroyed.Contains(item1) && !itemsToBeDestroyed.Contains(item2))
                                {
                                    if (item1.transform.position == item2.transform.position)
                                    {
                                        itemsToBeDestroyed.Add(item2);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (GameObject item in itemsToBeDestroyed)
            {
                Destroy(item);
            }
        }

        void PlaceDoors()
        {
            int numberOfDoors = Random.Range(1, CellsInRoom.Count + 1);
            foreach (Cell cell in CellsInRoom)
            {
                foreach (GameObject item in cell.Edges)
                {
                    
                }
            }
        }

        void UpdateCellsElements()
        {
            foreach (Cell cell in CellsInRoom)
                cell.UpdateElements();
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