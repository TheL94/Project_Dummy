using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Rooms.Data;
using DumbProject.Rooms.Cells;
using DumbProject.UI;
using DG.Tweening;

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
        public RoomMovment RoomMovment;

        Vector3 _initialPosition;
        public Vector3 InitialPosition
        {
            get { return _initialPosition; }
            private set { _initialPosition = value; }
        }

        bool canRotate = true;

        public void Setup(RoomData _data, GridController _grid, RoomMovment _roomMovment)
        {
            RoomMovment = _roomMovment;        
            RoomMovment.Init(this);
            InitialPosition = transform.position;
            PlaceCells(_data, _grid);
        }

        public void Setup(RoomData _data, GridController _grid)
        {
            PlaceCells(_data, _grid);
        }

        public void LinkRoom()
        {

        }

        /// <summary>
        /// Funzione che contiene le azioni da eseguire quando la posizione in cui si vuole metere la stanza è invalida
        /// </summary>
        public void ResetPositionToInitialPosition()
        {
            transform.position = InitialPosition;
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
                if (!cell.CheckPosition(GameManager.I.MainGridCtrl))
                {
                    return false;
                }
                foreach (GridNode adjacentNode in cell.RelativeNode.AdjacentNodes)
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
                transform.DORotate(transform.up * 90f, 0.5f, RotateMode.LocalAxisAdd).OnComplete(() => 
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
           
        }

        /// <summary>
        /// Funzione che piazza la stanza sulla griglia nella UI
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_grid"></param>
        protected abstract void PlaceCells(RoomData _data, GridController _grid);
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