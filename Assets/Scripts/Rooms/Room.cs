using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Rooms.Data;
using DumbProject.Rooms.Cells;

namespace DumbProject.Rooms
{
    public class Room : MonoBehaviour
    {
        public List<Cell> RoomCells;

        [HideInInspector]
        public RoomMovment RoomMovment;

        Vector3 _startPosition;
        public Vector3 StartPosition
        {
            get { return _startPosition; }
            private set { _startPosition = value; }
        }

        bool canRotate = true;

        public void Setup(RoomData _data, GridController _grid)
        {
            RoomMovment = GetComponent<RoomMovment>();        
            RoomMovment.Init(this);
            StartPosition = transform.position;
            PlaceCells(_data, _grid);
        }

        public void Init()
        {

        }

        public void LinkRoom()
        {

        }

        public void Rotate()
        {
            if (canRotate)
            {
                canRotate = false;
    
            }
        }

        void PlaceCells(RoomData _data, GridController _grid)
        {
            switch (_data.Shape)
            {
                case RoomShape.T_Shape:
                    TShapeRoom(_grid, _data);
                    break;
                default:
                    break;
            }
        }

        void TShapeRoom(GridController _grid, RoomData _data)
        {
            Cell[] cells = new Cell[4];

            for (int i = 0; i < cells.Length; i++)
                cells[i] = Instantiate(_data.Cell);

            Instantiate(_data.CellTypes.CellWallOpenFront, _grid.GetSpecificGridNode(new GridPosition(1, 1)).WorldPosition, Quaternion.identity, cells[0].transform);
            Instantiate(_data.CellTypes.CellWallBack, _grid.GetSpecificGridNode(new GridPosition(2, 0)).WorldPosition, Quaternion.LookRotation(cells[0].ReltiveNode.WorldPosition), cells[1].transform);
            Instantiate(_data.CellTypes.CellWallOpenFront, _grid.GetSpecificGridNode(new GridPosition(2, 1)).WorldPosition, Quaternion.LookRotation(cells[0].ReltiveNode.WorldPosition), cells[2].transform);
            Instantiate(_data.CellTypes.CellWallOpenFront, _grid.GetSpecificGridNode(new GridPosition(2, 2)).WorldPosition, Quaternion.LookRotation(cells[0].ReltiveNode.WorldPosition), cells[3].transform);
        }
    }

    public enum RoomShape
    {
        T_Shape
    }
}