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
                if (_statusOfExploration != ExplorationStatus.NotInGame)
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

        public void UpdateLinks()
        {
            if (StatusOfExploration != ExplorationStatus.NotInGame)
            {
                CheckIfConnectionIsAvailable();
            }
            else
            {
                foreach (INetworkable INet in Links)
                {
                    INet.RemoveLinks(new List<INetworkable>() { this });
                }
            }
        }

        void CheckIfConnectionIsAvailable()
        {
            // relative node
            GridNode node = RelativeCell.RelativeNode;
            if((int)RelativeCell.RelativeRoom.StatusOfExploration > 1)
            {
                AddLinks(new List<INetworkable>() { node });
                node.AddLinks(new List<INetworkable>() { this });
            }

            //node in front
            GridNode nodeInFront = RelativeCell.RelativeNode.RelativeGrid.GetSpecificGridNode(GetNodeInFrontPosition());
            if (nodeInFront != null && nodeInFront.RelativeCell != null)
            {
                // se lo stato della stanza di fronte alla porta è esplorato o in esplorazione allora mi collego
                if ((int)nodeInFront.RelativeCell.RelativeRoom.StatusOfExploration > 1)
                {
                    AddLinks(new List<INetworkable>() { nodeInFront });
                    nodeInFront.AddLinks(new List<INetworkable>() { this });
                }
            }
        }
        #endregion

        //Implementazione di IInteractable
        #region IInteractable
        bool _isInteractable;
        public bool IsInteractable { get { return _isInteractable; } set { _isInteractable = value; } }

        public void Interact()
        {

        }
        #endregion

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
            else if (nodeInFront != null && nodeInFront.RelativeCell == null)
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
                    case ExplorationStatus.Unexplored:
                        Gizmos.color = Color.yellow;
                        break;
                    case ExplorationStatus.Explored:
                        Gizmos.color = Color.red;
                        break;
                    default:
                        break;
                }
                Gizmos.DrawWireCube(transform.position + new Vector3(0f, 6f, 0f), Vector3.one);

                foreach (Framework.Pathfinding.INetworkable node in Links)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(spacePosition, node.spacePosition);
                }
            }
        }
    }

    public enum EdgeType { Wall = 0, Door = 1 }
}