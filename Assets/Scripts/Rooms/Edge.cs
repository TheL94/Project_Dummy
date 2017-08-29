using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DumbProject.Grid;

namespace DumbProject.Rooms
{
    public class Edge : CellElement
    {
        [HideInInspector]
        public Edge CollidingEdge;

        Material emissiveMaterial;

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

        public override void SetGraphic(GameObject _graphic, Quaternion _rotation)
        {
            base.SetGraphic(_graphic, _rotation);
            SetEmissiveMaterial(_graphic);
        }

        public void ToggleEmissive(bool _toggle, Color _color)
        {
            if (emissiveMaterial == null)
                return;

            emissiveMaterial.SetColor("_EmissionColor", _color);

            if (_toggle)
                emissiveMaterial.EnableKeyword("_EMISSION");
            else
                emissiveMaterial.DisableKeyword("_EMISSION");
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

        void SetEmissiveMaterial(GameObject _graphic)
        {
            // TODO : logica da rivedere
            MeshRenderer renderer = _graphic.GetComponentInChildren<MeshRenderer>();
            List<Material> materials = null;
            if (renderer != null)
                materials = renderer.materials.ToList();
            if (materials != null)
                emissiveMaterial = materials.Find(m => m.name.Contains("emissive_mat"));
        }
    }
}