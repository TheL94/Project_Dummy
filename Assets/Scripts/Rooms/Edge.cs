using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    public class Edge : MonoBehaviour
    {
        EdgeType _type = EdgeType.Wall;
        public EdgeType Type
        {
            get { return _type; }
            set
            {
                _type = value;
            }
        }
        [HideInInspector]
        public Edge CollidingEdge;
        [HideInInspector]
        public Cell RelativeCell;

        GameObject graphic;

        public void Setup(Cell _relativeCell)
        {
            RelativeCell = _relativeCell;
        }

        public void SetGraphic(GameObject _graphic, Quaternion _rotation)
        {
            graphic = _graphic;
            graphic.transform.position = transform.position;
            graphic.transform.rotation = _rotation;
            graphic.transform.parent = transform;
        }

        public void CheckCollisionWithOtherEdges(GridController _grid)
        {
            GridNode nodeInFront = _grid.GetSpecificGridNode(GetNodeInFrontPosition());
            List<Edge> edgesInFrontCell = new List<Edge>();
            if (nodeInFront != null && nodeInFront.RelativeCell != null)
            {
                edgesInFrontCell = nodeInFront.RelativeCell.GetEdgesList();
                foreach (Edge edgeInFront in edgesInFrontCell)
                {
                    if (Vector3.Distance(edgeInFront.transform.position, transform.position) <= RelativeCell.RelativeRoom.Data.PenetrationOffset)
                    {
                        CollidingEdge = edgeInFront;
                    }
                }
            }
            else if(nodeInFront != null && nodeInFront.RelativeCell == null)
            {
                CollidingEdge = null;
            }
        }

        public Vector3 GetNodeInFrontPosition()
        {
            return (transform.position * 2) - RelativeCell.transform.position;
        }

        private void OnDisable()
        {
            graphic.SetActive(false);
            graphic = null;
        }
    }

    public enum EdgeType { Wall = 0, Door = 1 }
}