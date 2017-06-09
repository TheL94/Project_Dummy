using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;

namespace DumbProject.Rooms.Cells
{
    public class Cell : MonoBehaviour
    {
        public GridNode RelativeNode;
        MeshRenderer[] childrenMesh;

        private void Start()
        {
            childrenMesh = GetComponentsInChildren<MeshRenderer>();
        }

        public Cell PlaceCellInUI(GridNode _relativeNode, Quaternion _rotation, Transform _parent)
        {
            RelativeNode = _relativeNode;
            transform.position = RelativeNode.WorldPosition;
            transform.rotation = _rotation;
            transform.parent = _parent;
            return this;
        }

        public Cell PlaceCellInMainGrid(GridNode _relativeNode)
        {
            RelativeNode = _relativeNode;
            transform.position = RelativeNode.WorldPosition;
            RelativeNode.RelativeCell = this;
            return this;
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