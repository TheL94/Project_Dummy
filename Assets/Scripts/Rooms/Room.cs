using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Rooms.Data;
using DumbProject.Rooms.Cells;

namespace DumbProject.Rooms
{
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

        public void ResetPositionToInitialPosition()
        {
            transform.position = InitialPosition;
        }

        public void PlaceAction()
        {
            transform.parent = null;
            Destroy(RoomMovment);
        }

        public void Rotate()
        {
            if (canRotate)
            {
                canRotate = false;
            }
        }

        public abstract void PlaceAsMainRoom(RoomData _data, GridController _grid);

        protected abstract void PlaceCells(RoomData _data, GridController _grid);
    }                                      
                                           
    public enum RoomShape
    {
        T_Shape
    }
}