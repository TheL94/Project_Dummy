using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Rooms.Data;
using DumbProject.Rooms.Cells;
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

        public void Init()
        {

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
            transform.parent = null;
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
        /// Funzione che piazza la stanza direttamente sulla griglia principale
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_grid"></param>
        public abstract void PlaceAsMainRoom(RoomData _data, GridController _grid);

        /// <summary>
        /// Funzione che piazza la stanza sulla griglia nella UI
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_grid"></param>
        protected abstract void PlaceCells(RoomData _data, GridController _grid);
    }                                      
                                           
    public enum RoomShape
    {
        T_Shape
    }
}