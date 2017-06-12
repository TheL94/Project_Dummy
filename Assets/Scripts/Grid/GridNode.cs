using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms.Cells;
using System;

namespace DumbProject.Grid
{
    public class GridNode : ICellNode
    {
        public GridController RelativeGrid;
        public GridPosition GridPosition;
        public Vector3 WorldPosition;

        Cell _relativeCell;
        public Cell RelativeCell
        {
            get { return _relativeCell; }
            set { _relativeCell = value; }
        }
        public List<GridNode> AdjacentNodes;

        public GridNode() { }

        public GridNode(GridController _myGrid, GridPosition _gridPosition, Vector3 _worldPosition)
        {
            RelativeGrid = _myGrid;
            GridPosition = _gridPosition;
            WorldPosition = _worldPosition;
        }


        public void Init(List<GridNode> _adjacentNodes)
        {
            AdjacentNodes = _adjacentNodes;
        }
    }

    public struct GridPosition
    {
        public int x;
        public int z;

        public GridPosition(int _x, int _z)
        {
            x = _x;
            z = _z;
        }
    }
}