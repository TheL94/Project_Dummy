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
        [HideInInspector]
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
            Cell centralCell = Instantiate(_data.CellPrefab, _grid.GetSpecificGridNode(new GridPosition(1, 1)).WorldPosition, Quaternion.identity, transform);
            Cell leftTopCell = Instantiate(_data.CellPrefab, _grid.GetSpecificGridNode(new GridPosition(2, 0)).WorldPosition, Quaternion.LookRotation(centralCell.ReltiveNode.WorldPosition), transform);
            Cell centralTopCell = Instantiate(_data.CellPrefab, _grid.GetSpecificGridNode(new GridPosition(2, 1)).WorldPosition, Quaternion.LookRotation(centralCell.ReltiveNode.WorldPosition), transform);
            Cell rightTopCell = Instantiate(_data.CellPrefab, _grid.GetSpecificGridNode(new GridPosition(2, 2)).WorldPosition, Quaternion.LookRotation(centralCell.ReltiveNode.WorldPosition), transform);

            Instantiate(_data.CellTypes.CellWallOpenFront, centralCell.transform.position, centralCell.transform.rotation, centralCell.transform);
            Instantiate(_data.CellTypes.CellWallOpenFront, leftTopCell.transform.position, leftTopCell.transform.rotation, leftTopCell.transform);
            Instantiate(_data.CellTypes.CellWallBack, centralTopCell.transform.position, centralTopCell.transform.rotation, centralTopCell.transform);
            Instantiate(_data.CellTypes.CellWallOpenFront, rightTopCell.transform.position, rightTopCell.transform.rotation, rightTopCell.transform);

            RoomCells = new List<Cell>() { centralCell, leftTopCell, centralTopCell, rightTopCell };
    }                                  
    }                                      
                                           
    public enum RoomShape
    {
        T_Shape
    }
}