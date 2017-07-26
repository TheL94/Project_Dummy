using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Generic;
using Framework.Pathfinding;
using DumbProject.Items;
using System;

namespace DumbProject.Rooms
{
    public class Edge : CellElement
    {
        [HideInInspector]
        public Edge CollidingEdge;

        /// <summary>
        /// Funzione che controlla la collisione con altri edge
        /// </summary>
        /// <param name="_grid"></param>
        public void CheckCollisionWithOtherEdges(GridController _grid)
        {
            GridNode nodeInFront = _grid.GetSpecificGridNode(GetOppositeOfRelativeCellPosition());
            List<Edge> edgesInFrontCell = new List<Edge>();
            if (nodeInFront != null && nodeInFront.RelativeCell != null)
            {
                edgesInFrontCell.AddRange(nodeInFront.RelativeCell.Edges);
                edgesInFrontCell.AddRange(nodeInFront.RelativeCell.Doors.ConvertAll(d => d as Edge));
                foreach (Edge edgeInFront in edgesInFrontCell)
                {
                    if (Vector3.Distance(edgeInFront.transform.position, transform.position) <= RelativeCell.RelativeRoom.Data.PenetrationOffset)
                    {
                        CollidingEdge = edgeInFront;
                        return;
                    }
                    else
                        CollidingEdge = null;
                }
            }
            else if (nodeInFront != null && nodeInFront.RelativeCell == null)
            {
                CollidingEdge = null;
            }
        }

        /// <summary>
        /// Funzione che ritorna la posizione che sta di fronte a se stesso
        /// </summary>
        /// <returns></returns>
        public Vector3 GetOppositeOfRelativeCellPosition()
        {
            return GetFrontPosition(RelativeCell.transform.position);
        }

        public Vector3 GetFrontPosition(Vector3 _position)
        {
            return (transform.position * 2) - _position;
        }

        /// <summary>
        /// Funzione che disabilita l'oggetto
        /// </summary>
        /// <param name="_avoidDestruction"></param>
        public override void DisableAndDestroyObject(bool _avoidDestruction = false)
        {
            RelativeCell.Edges.Remove(this);
            base.DisableAndDestroyObject(_avoidDestruction);
        }
    }
}