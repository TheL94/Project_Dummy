using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Grid
{
    public class GridNode
    {
        public GridPosition GridPosition;
        public Vector3 WorldPosition;
        public Cell RelativeCell;

        public List<GridNode> AdjacentNodes;

        public GridNode() { }

        public GridNode(GridPosition _gridPosition, Vector3 _worldPosition)
        {
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