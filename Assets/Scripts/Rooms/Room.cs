using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Rooms.Data;
using DumbProject.Rooms.Cells;
using DumbProject.UI;
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

        Vector3 _initialPosition;
        Tweener rotationTween;
        public Vector3 InitialPosition
        {
            get { return _initialPosition; }
            private set { _initialPosition = value; }
        }

        bool canRotate = true;

        public void Setup(RoomData _data, GridController _grid, RoomMovement _roomMovment)
        {
            RoomMovment = _roomMovment;        
            RoomMovment.Init(this);
            InitialPosition = transform.position;
            Setup(_data, _grid);
        }

        public void Setup(RoomData _data, GridController _grid)
        {
            PlaceCells(_data, _grid);
            TrimCellWalls();
            TrimCellPillars();
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
        /// Piazza l'elemento passatogli da UIRoomController all'interno della stanza
        /// </summary>
        public void PlaceItemInside(GameObject _item)
        {
            int tempNum = Random.Range(0, CellsInRoom.Count);
            Instantiate(_item, new Vector3(CellsInRoom[tempNum].transform.position.x, CellsInRoom[tempNum].transform.position.y +2, CellsInRoom[tempNum].transform.position.z), Quaternion.identity, transform);
            Debug.Log("Instanziata");
        }

        /// <summary>
        /// Funzione che piazza la stanza sulla griglia nella UI
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_grid"></param>
        protected abstract void PlaceCells(RoomData _data, GridController _grid);
        
        /// <summary>
        /// Funzione che rimuove i muri se sono nella stessa posizione
        /// </summary>
        protected void TrimCellWalls()
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
                                    if (item1.transform.position == item2.transform.position)
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
        protected void TrimCellPillars()
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