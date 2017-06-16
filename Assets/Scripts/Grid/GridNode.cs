using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms.Cells;
using Framework.Pathfinding;
using System;

namespace DumbProject.Grid
{
    public class GridNode : ICellNode, INetworkable
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
            //Provvisoria connessione di tutti gli adiacenti come link del network
            //foreach (INetworkable node in AdjacentNodes)
            //{
            //    AddLinks(new List<INetworkable>() { node });
            //}
        }

        //Implementazione di INetworkable
        #region INetworkable
        public Vector3 spacePosition { get { return WorldPosition; } set { } }

        public List<INetworkable> Links { get; set; }

        public void AddLinks(List<INetworkable> _newLinks) {
            foreach (INetworkable _INet in _newLinks)
            {
                if (!Links.Contains(_INet))
                    Links.Add(_INet);
            }
        }

        public void RemoveLinks(List<INetworkable> _linksToRemove) {
            foreach (INetworkable _INet in _linksToRemove)
            {
                if (Links.Contains(_INet))
                    Links.Remove(_INet);
            }
        }
        #endregion
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