using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Grid
{
    public class NetNode : GenericNode
    {
        IInteractable _relativeInteractable;
        public IInteractable RelativeInteractable
        {
            get { return _relativeInteractable; }
            private set { _relativeInteractable = value; }
        }

        public NetNode(GridController _myGrid, Vector3 _worldPosition) : base(_myGrid, _worldPosition)
        {
            RelativeGrid = _myGrid;
            WorldPosition = _worldPosition;
        }

        public void Init(List<GenericNode> _adjacentNodes, IInteractable _relativeInteractable)
        {
            AdjacentNodes = _adjacentNodes;
            RelativeInteractable = _relativeInteractable;
        }
    }
}

