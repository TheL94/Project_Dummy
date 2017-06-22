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
        public EdgeType WithWhatIsColliding = EdgeType.None;
        [HideInInspector]
        public Cell RelativeCell;

        public void Setup(Cell _relativeCell)
        {
            RelativeCell = _relativeCell;
        }

        public void CheckCollisionWithOtherEdges(GridController _grid)
        {
            GridNode node = _grid.GetSpecificGridNode(GetProbePosition());
            List<Edge> edgesInFrontCell = new List<Edge>();
            if (node != null && node.RelativeCell != null)
            {
                edgesInFrontCell = node.RelativeCell.GetEdgesList();
                foreach (Edge edge in edgesInFrontCell)
                {
                    if (Vector3.Distance(edge.transform.position, transform.position) <= RelativeCell.RelativeRoom.Data.PenetrationOffset)
                    {
                        if(edge.Type == EdgeType.Door)
                        {
                            WithWhatIsColliding = EdgeType.Door;
                        }
                        else if(edge.Type == EdgeType.Wall)
                        {
                            WithWhatIsColliding = EdgeType.Wall;
                        }
                        else
                        {
                            WithWhatIsColliding = EdgeType.None;
                        }
                    }
                }
            }
            else if(node != null && node.RelativeCell == null)
            {
                WithWhatIsColliding = EdgeType.None;
            }
        }

        Vector3 GetProbePosition()
        {
            return (transform.position * 2) - RelativeCell.transform.position;
        }
    }

    public enum EdgeType { None = 0, Wall = 1, Door = 2 }
}