using DumbProject.Generic;
using DumbProject.Grid;
using Framework.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Rooms
{
    public class Door : Edge, INetworkable, IInteractable
    {
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
            //if (StatusOfExploration != ExplorationStatus.NotInGame)
            //{
            LinkIfConnectionIsAvailable();
            //}
            //else
            //{
            //    foreach (INetworkable INet in Links)
            //    {
            //        INet.RemoveLinks(new List<INetworkable>() { this });
            //    }
            //}
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
            GridNode nodeInFront = RelativeCell.RelativeNode.RelativeGrid.GetSpecificGridNode(GetOppositeOfRelativeCellPosition());
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

        public void Interact(AIActor _actor)
        {
            GridNode actorNode = _actor.CurrentNode as GridNode;
            GridNode nodeInFront = actorNode.RelativeGrid.GetSpecificGridNode(GetFrontPosition(actorNode.WorldPosition));
            Room nextRoom = null;
            if (nodeInFront.RelativeCell != null)
            {
                //Se trova una stanza la collega e aggiorna lo stato delle stanze/porte nel dungeon
                nextRoom = nodeInFront.RelativeCell.RelativeRoom;
                if (nextRoom.StatusOfExploration == ExplorationStatus.NotExplored)
                {
                    nextRoom.StatusOfExploration = ExplorationStatus.InExploration;
                }
            }
            else
            {
                //Se non trova nulla, semplicemente aggiunge solo il link della cella davanti
                AddLinks(new List<INetworkable>() { nodeInFront });
            }


            //if(StatusOfExploration == ExplorationStatus.NotExplored)
            //{
            //    Room room = RelativeCell.RelativeRoom;
            //    if (room.StatusOfExploration == ExplorationStatus.NotExplored)
            //    {
            //        room.StatusOfExploration = ExplorationStatus.InExploration;
            //    }


            //        Room roomInFront = nodeInFront.RelativeCell.RelativeRoom;
            //        if (nodeInFront != null && nodeInFront.RelativeCell != null)
            //        {
            //            if (room.StatusOfExploration == ExplorationStatus.NotExplored)
            //            {
            //                roomInFront.StatusOfExploration = ExplorationStatus.InExploration;
            //            }
            //        }
            //    }
            //}

            //se uno dei nodi collegati della porta è quello in cui c'è l'actor prende l'altro
            INetworkable next = _actor.CurrentNode;
            if (next.spacePosition == Links[0].spacePosition)
                next = Links[1];
            else
                next = Links[0];

            _actor.INetworkableObjective = next;
            IsInteractable = false;
        }
        #endregion

        private void OnDrawGizmos()
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

            //Gizmos.color = Color.magenta;
            //if(RelativeCell.RelativeNode.RelativeGrid.GetSpecificGridNode(GetFrontPosition()) != null)
            //    Gizmos.DrawLine(RelativeCell.RelativeNode.RelativeGrid.GetSpecificGridNode(GetFrontPosition()).WorldPosition + new Vector3(0f, 6f, 0f), transform.position + new Vector3(0f, 6f, 0f));           
        }
    }
}

