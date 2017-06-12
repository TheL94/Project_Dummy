﻿using System.Collections;
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
                if(_relativeNode != null && _relativeNode.RelativeCell != null)
                    _relativeNode.RelativeCell = null;

                _relativeNode = value;
                if(_relativeNode != null)
                {
                    transform.position = RelativeNode.WorldPosition;
                    RelativeNode.RelativeCell = this;
                }
            }
        }

        MeshRenderer[] childrenMesh;
        Room relativeRoom;

        private void Start()
        {
            childrenMesh = GetComponentsInChildren<MeshRenderer>();
        }

        public Cell PlaceCell(GridNode _relativeNode, Quaternion _rotation, Room _relativeRoom)
        {
            RelativeNode = _relativeNode;          
            transform.rotation = _rotation;
            relativeRoom = _relativeRoom;
            transform.parent = relativeRoom.transform;
            return this;
        }

        public bool CheckPosition(GridController _grid)
        {
            GridNode node = _grid.GetSpecificGridNode(transform.position);
            if (node != null && node.RelativeCell == null)
                return true;

            return false;
        }

        public GridNode GetMyPositionOnGrid(GridController _grid)
        {
            return _grid.GetSpecificGridNode(transform.position);
        }

        public void ResetRelativeNode()
        {
            GridController grid = RelativeNode.RelativeGrid;
            RelativeNode = grid.GetSpecificGridNode(transform.position);
        }

        public void SetRelativeNode(GridNode _relativeNode)
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