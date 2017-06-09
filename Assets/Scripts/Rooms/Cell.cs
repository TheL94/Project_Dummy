using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;

namespace DumbProject.Rooms.Cells
{
    public class Cell : MonoBehaviour
    {
        GridNode _relativeNode;
        public GridNode RelativeNode
        {
            get { return _relativeNode; }
            set
            {
                _relativeNode = value;
                if(_relativeNode != null)
                {
                    transform.position = RelativeNode.WorldPosition;
                    RelativeNode.RelativeCell = this;
                }
            }
        }

        MeshRenderer[] childrenMesh;

        private void Start()
        {
            childrenMesh = GetComponentsInChildren<MeshRenderer>();
        }

        public Cell PlaceCellInUI(GridNode _relativeNode, Quaternion _rotation, Transform _parent)
        {
            RelativeNode = _relativeNode;          
            transform.rotation = _rotation;
            transform.parent = _parent;
            return this;
        }

        public Cell PlaceCellInMainGrid(GridNode _relativeNode, Quaternion _rotation, Transform _parent)
        {
            RelativeNode = _relativeNode;
            transform.rotation = _rotation;
            transform.parent = _parent;
            return this;
        }

        public void ResetRelativeNode()
        {
            GridController grid = RelativeNode.MyGrid;
            RelativeNode = grid.GetSpecificGridNode(transform.position);
        }

        public void ResetRelativeNode(GridNode _relativeNode)
        {
            RelativeNode = _relativeNode;
        }

        public void ShowInvalidPosition(bool _isInvalid)
        {
            if(_isInvalid)
                foreach (MeshRenderer mesh in childrenMesh)
                    mesh.material.color = Color.red;
            else
                foreach (MeshRenderer mesh in childrenMesh)
                    mesh.material.color = Color.white;
        }
    }
}