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
    public class Edge : MonoBehaviour, INetworkable, IInteractable
    {
        [HideInInspector]
        public EdgeType Type = EdgeType.Wall;

        ExplorationStatus _statusOfExploration = ExplorationStatus.NotInGame;
        public ExplorationStatus StatusOfExploration
        {
            get { return _statusOfExploration; }
            set
            {
                _statusOfExploration = value;
                UpdateLinks();
            }
        }

        [HideInInspector]
        public Edge CollidingEdge;
        [HideInInspector]
        public Cell RelativeCell;

        GameObject graphic;

        //Implementazione di INetworkable
        #region INetworkable
        public Vector3 spacePosition { get { return transform.position; } set { } }
        List<INetworkable> _links = new List<INetworkable>();
        public List<INetworkable> Links { get { return _links; } set { _links = value; } }

        public void AddLinks(List<INetworkable> _newLinks)
        {
            foreach (INetworkable _INet in _newLinks)
            {
                if (!Links.Contains(_INet))
                    Links.Add(_INet);
            }
        }

        public void RemoveLinks(List<INetworkable> _linksToRemove)
        {
            foreach (INetworkable _INet in _linksToRemove)
            {
                if (Links.Contains(_INet))
                    Links.Remove(_INet);
            }
        }

        /// <summary>
        /// Funzione che aggiorna i links
        /// </summary>
        public void UpdateLinks()
        {
            if (StatusOfExploration != ExplorationStatus.NotInGame)
            {
                LinkIfConnectionIsAvailable();
            }
            else
            {
                foreach (INetworkable INet in Links)
                {
                    INet.RemoveLinks(new List<INetworkable>() { this });
                }
            }
        }

        /// <summary>
        /// Collega, se possibile e se valido per ogni nodo, la stanza con i due nodi vicini
        /// </summary>
        void LinkIfConnectionIsAvailable()
        {
            // relative node
            GridNode node = RelativeCell.RelativeNode;
            //se lo stato della stanza di fronte alla porta è esplorato o in esplorazione allora mi collego
            if ((RelativeCell.RelativeRoom.StatusOfExploration == ExplorationStatus.InExploration) ||
                (RelativeCell.RelativeRoom.StatusOfExploration == ExplorationStatus.Explored))
            {
                AddLinks(new List<INetworkable>() { node });
                node.AddLinks(new List<INetworkable>() { this });
            }

            //node in front
            GridNode nodeInFront = RelativeCell.RelativeNode.RelativeGrid.GetSpecificGridNode(GetFrontPosition());
            if (nodeInFront != null && nodeInFront.RelativeCell != null)
            {
                //se lo stato della stanza di fronte alla porta è esplorato o in esplorazione allora mi collego
                if ((nodeInFront.RelativeCell.RelativeRoom.StatusOfExploration == ExplorationStatus.InExploration) ||
                    (nodeInFront.RelativeCell.RelativeRoom.StatusOfExploration == ExplorationStatus.Explored))
                {
                    AddLinks(new List<INetworkable>() { nodeInFront });
                    nodeInFront.AddLinks(new List<INetworkable>() { this });
                }
            }
        }
        #endregion

        //Implementazione di IInteractable
        #region IInteractable
        public Transform Transf { get { return transform; } }
        bool _isInteractable = true;
        public bool IsInteractable { get { return _isInteractable; } set { _isInteractable = value; } }

        public void Interact()
        {
            if (Type == EdgeType.Wall)
            {
                IsInteractable = false;
                return; 
            }

            if(StatusOfExploration == ExplorationStatus.NotExplored)
            {
                Room room = RelativeCell.RelativeRoom;
                if (room.StatusOfExploration == ExplorationStatus.NotExplored)
                {
                    room.StatusOfExploration = ExplorationStatus.InExploration;
                }

                GridNode nodeInFront = RelativeCell.RelativeNode.RelativeGrid.GetSpecificGridNode(GetFrontPosition());
                Room roomInFront = nodeInFront.RelativeCell.RelativeRoom;
                if (nodeInFront != null && nodeInFront.RelativeCell != null)
                {
                    if (room.StatusOfExploration == ExplorationStatus.NotExplored)
                    {
                        roomInFront.StatusOfExploration = ExplorationStatus.InExploration;
                    }
                }

                IsInteractable = false;
            }
        }
        #endregion

        public void Setup(Cell _relativeCell)
        {
            RelativeCell = _relativeCell;
        }

        /// <summary>
        /// Funzione che mette l'oggetto di grafica nella posizione giusta e lo mette figlio dell'edge
        /// </summary>
        /// <param name="_graphic"></param>
        /// <param name="_rotation"></param>
        public void SetGraphic(GameObject _graphic, Quaternion _rotation)
        {
            graphic = _graphic;
            graphic.transform.position = transform.position;
            graphic.transform.rotation = _rotation;
            graphic.transform.parent = transform;
        }

        /// <summary>
        /// Funzione che controlla la collisione con altri edge
        /// </summary>
        /// <param name="_grid"></param>
        public void CheckCollisionWithOtherEdges(GridController _grid)
        {
            GridNode nodeInFront = _grid.GetSpecificGridNode(GetFrontPosition());
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
            else if (nodeInFront != null && nodeInFront.RelativeCell == null)
            {
                CollidingEdge = null;
            }
        }

        /// <summary>
        /// Funzione che ritorna la posizione che sta di fronte a se stesso
        /// </summary>
        /// <returns></returns>
        public Vector3 GetFrontPosition()
        {
            return (transform.position * 2) - RelativeCell.transform.position;
        }

        /// <summary>
        /// Funzione che continene
        /// </summary>
        public void DisableActions()
        {
            if (graphic != null)
            {
                graphic.SetActive(false);
                graphic = null;
            }
            StatusOfExploration = ExplorationStatus.NotInGame;
        }

        private void OnDrawGizmos()
        {
            if (Type == EdgeType.Door)
            {
                switch (StatusOfExploration)
                {
                    case ExplorationStatus.NotInGame:
                        Gizmos.color = Color.white;
                        break;
                    case ExplorationStatus.Unavailable:
                        Gizmos.color = Color.red;
                        break;
                    case ExplorationStatus.NotExplored:
                        Gizmos.color = Color.yellow;
                        break;
                    case ExplorationStatus.Explored:
                        Gizmos.color = Color.green;
                        break;
                    default:
                        break;
                }
                Gizmos.DrawWireCube(transform.position + new Vector3(0f, 6f, 0f), Vector3.one);

                Gizmos.color = Color.cyan;
                foreach (INetworkable node in Links)
                {
                    Gizmos.DrawLine(spacePosition + new Vector3(0f, 1f, 0f), node.spacePosition + new Vector3(0f, 1f, 0f));
                }

                Gizmos.color = Color.magenta;
                if(RelativeCell.RelativeNode.RelativeGrid.GetSpecificGridNode(GetFrontPosition()) != null)
                    Gizmos.DrawLine(RelativeCell.RelativeNode.RelativeGrid.GetSpecificGridNode(GetFrontPosition()).WorldPosition + new Vector3(0f, 6f, 0f), transform.position + new Vector3(0f, 6f, 0f));
            }
        }
    }

    public enum EdgeType { Wall = 0, Door = 1 }
}