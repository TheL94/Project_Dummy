using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using Framework.Pathfinding;
using System;

namespace DumbProject.Grid
{
    public class GridNode : NetNode
    {
        public GridPosition GridPosition;

        Cell _relativeCell;
        public Cell RelativeCell
        {
            get { return _relativeCell; }
            set { _relativeCell = value; }
        }

        public GridNode(GridController _myGrid, GridPosition _gridPosition, Vector3 _worldPosition) : base(_myGrid, _worldPosition)
        {
            RelativeGrid = _myGrid;
            GridPosition = _gridPosition;
            WorldPosition = _worldPosition;
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

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(GridPosition gp1, GridPosition gp2)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(gp1, gp2))
                return true;
            
            // If one is null, but not both, return false.
            if (((object)gp1 == null) || ((object)gp2 == null))
                return false;

            // Return true if the fields match:
            return gp1.x == gp2.x && gp1.z == gp2.z;
        }

        public static bool operator !=(GridPosition gp1, GridPosition gp2)
        {
            return !(gp1 == gp2);
        }
    }
}