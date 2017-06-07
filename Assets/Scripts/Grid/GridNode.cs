using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms.Cells;
using System;

namespace DumbProject.Grid
{
    public class GridNode : ICellNode
    {
        public GridPosition GridPosition;
        public Vector3 WorldPosition;

        public List<GridNode> AdjacentNodes;

        public GridNode() { }

        public GridNode(GridPosition _gridPosition, Vector3 _worldPosition)
        {
            GridPosition = _gridPosition;
            WorldPosition = _worldPosition;
        }

        Cell _relativeCell;
        public Cell RelativeCell
        {
            get { return _relativeCell; }
            set { _relativeCell = value; }
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