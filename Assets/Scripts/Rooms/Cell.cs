using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;

namespace DumbProject.Rooms.Cells
{
    public class Cell : MonoBehaviour
    {
        public GridNode RelativeNode;

        public Cell PlaceCell(GridNode _relativeNode, Quaternion _rotation, Transform _parent)
        {
            RelativeNode = _relativeNode;
            transform.position = RelativeNode.WorldPosition;
            transform.rotation = _rotation;
            transform.parent = _parent;
            return this;
        }
    }
}