using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Pathfinding;

namespace DumbProject.Grid
{
    public class GenericNode : INetworkable
    {
        public GridController RelativeGrid;
        public Vector3 WorldPosition;

        public List<GenericNode> AdjacentNodes;

        public GenericNode(GridController _myGrid, Vector3 _worldPosition)
        {
            RelativeGrid = _myGrid;
            WorldPosition = _worldPosition;
        }

        public void Init(List<GenericNode> _adjacentNodes)
        {
            AdjacentNodes = _adjacentNodes;
        }

        //Implementazione di INetworkable
        #region INetworkable
        public Vector3 spacePosition { get { return WorldPosition; } set { } }
        List<INetworkable> _links = new List<INetworkable>();
        public List<INetworkable> Links
        {
            get { return _links; }
            set { _links = value; }
        }

        public void AddLinks(List<INetworkable> _newLinks)
        {
            foreach (INetworkable _INet in _newLinks)
            {
                if (!Links.Contains(_INet))
                    Links.Add(_INet);
            }
        }

        public void RemoveLinks(List<INetworkable> _linksToRemove)
        {
            foreach (INetworkable _INet in _linksToRemove)
            {
                if (Links.Contains(_INet))
                    Links.Remove(_INet);
            }
        }
        #endregion
    }
}