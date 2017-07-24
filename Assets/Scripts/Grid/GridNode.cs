using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using Framework.Pathfinding;
using System;

namespace DumbProject.Grid
{
    public class GridNode : NetNode, ICellNode
    {
        public GridPosition GridPosition;

        Cell _relativeCell;
        public Cell RelativeCell
        {
            get { return _relativeCell; }
            set { _relativeCell = value; }
        }

        new public List<GridNode> AdjacentNodes;

        public GridNode(GridController _myGrid, GridPosition _gridPosition, Vector3 _worldPosition) : base(_myGrid, _worldPosition)
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