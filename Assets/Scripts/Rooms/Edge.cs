using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using DumbProject.Grid;


namespace DumbProject.Rooms.Cells
{
    public class Edge : MonoBehaviour
    {
        [HideInInspector]
        public EdgeType Type = EdgeType.Wall;
        [HideInInspector]
        public Edge CollidingEdge;
        [HideInInspector]
        public Cell RelativeCell;

        public void Setup(Cell _relativeCell)
        {
            RelativeCell = _relativeCell;
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

        Vector3 GetNodeInFrontPosition()
        {
            return (transform.position * 2) - RelativeCell.transform.position;
        }
    }

    public enum EdgeType { Wall = 0, Door = 1 }
}