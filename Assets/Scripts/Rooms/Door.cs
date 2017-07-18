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
        public ExplorationStatus StatusOfExploration
        {
            get { return EvaluateStatus(); }
        }

        Cell[] _adjacentCells = new Cell[2];
        public Cell[] AdjacentCells { get { return _adjacentCells; } }

        public override void Setup(Cell _relativeCell)
        {
            base.Setup(_relativeCell);
            AddAjdacentCell(RelativeCell);
        }

        /// <summary>
        /// Funzione che rimove la door dalla cell
        /// </summary>
        public override void DisableEdge()
        {
            if (graphic != null)
            {
                graphic.SetActive(false);
                graphic = null;
            }
            RelativeCell.Doors.Remove(this);
            gameObject.SetActive(false);
            Debug.Log("Door spenta");
        }

        /// <summary>
        /// Funzione che aggiunge una cella come adiacente alla porta
        /// </summary>
        /// <param name="_adjacentCell"></param>
        public void AddAjdacentCell(Cell _adjacentCell)
        {
            for (int i = 0; i < _adjacentCells.Length; i++)
            {
                if (_adjacentCells[i] == null)
                {
                    _adjacentCells[i] = _adjacentCell;
                    return;
                }
            }
        }

        /// <summary>
        /// Update INetworkable links based on the exploration status
        /// </summary>
        public void UpdateLinks()
        {
            for (int i = 0; i < AdjacentCells.Length; i++)
            {
                if (AdjacentCells[i] == null)
                    continue;

                if(AdjacentCells[i].RelativeRoom.StatusOfExploration == ExplorationStatus.Explored || AdjacentCells[i].RelativeRoom.StatusOfExploration == ExplorationStatus.InExploration)
                {
                    INetworkable networkable = AdjacentCells[i].RelativeNode as INetworkable;
                    AddLinks(new List<INetworkable>() { networkable });
                    AdjacentCells[i].RelativeNode.AddLinks(new List<INetworkable>() { this });
                }
            }
        }

        /// <summary>
        /// Evaluate the status of the door based on the status of his adjacent rooms
        /// </summary>
        /// <returns></returns>
        ExplorationStatus EvaluateStatus()
        {
            ExplorationStatus status = ExplorationStatus.NotInGame;

            if (AdjacentCells[1] == null)
            {
                switch (AdjacentCells[0].RelativeRoom.StatusOfExploration)
                {
                    case ExplorationStatus.NotInGame:
                        break;
                    case ExplorationStatus.Unavailable:
                    case ExplorationStatus.NotExplored:    
                        return status = ExplorationStatus.Unavailable;
                    case ExplorationStatus.InExploration:
                    case ExplorationStatus.Explored:
                        return status = ExplorationStatus.NotExplored;
                }
            }
            else
            {
                switch (AdjacentCells[0].RelativeRoom.StatusOfExploration)
                {
                    case ExplorationStatus.NotInGame:
                        break;
                    case ExplorationStatus.Unavailable:
                        switch (AdjacentCells[1].RelativeRoom.StatusOfExploration)
                        {
                            case ExplorationStatus.Unavailable:
                            case ExplorationStatus.NotExplored:
                                return status = ExplorationStatus.Unavailable;
                            default:
                                Debug.LogWarning("Porta in una combinazione di stanze non valutata!");
                                break;
                        }
                        break;
                    case ExplorationStatus.NotExplored:
                        switch (AdjacentCells[1].RelativeRoom.StatusOfExploration)
                        {
                            case ExplorationStatus.Unavailable:
                            case ExplorationStatus.NotExplored:
                                return status = ExplorationStatus.Unavailable;
                            case ExplorationStatus.InExploration:
                            case ExplorationStatus.Explored:
                                return status = ExplorationStatus.NotExplored;
                            default:
                                Debug.LogWarning("Porta in una combinazione di stanze non valutata!");
                                break;
                        }
                        break;
                    case ExplorationStatus.InExploration:
                        switch (AdjacentCells[1].RelativeRoom.StatusOfExploration)
                        {
                            case ExplorationStatus.NotExplored:
                                return status = ExplorationStatus.NotExplored;
                            case ExplorationStatus.Explored:
                                return status = ExplorationStatus.Explored;
                            default:
                                Debug.LogWarning("Porta in una combinazione di stanze non valutata!");
                                break;
                        }
                        break;
                    case ExplorationStatus.Explored:
                        switch (AdjacentCells[1].RelativeRoom.StatusOfExploration)
                        {
                            case ExplorationStatus.NotExplored:
                                return status = ExplorationStatus.NotExplored;
                            case ExplorationStatus.InExploration:
                            case ExplorationStatus.Explored:
                                return status = ExplorationStatus.Explored;
                            default:
                                Debug.LogWarning("Porta in una combinazione di stanze non valutata!");
                                break;
                        }
                        break;
                }
            }
            return status;
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
        #endregion

        //Implementazione di IInteractable
        #region IInteractable
        public Transform Transf { get { return transform; } }
        bool _isInteractable = true;
        public bool IsInteractable { get { return _isInteractable; } set { _isInteractable = value; } }

        public void Interact(AIActor _actor)
        {
            GridNode actorNode = _actor.CurrentNode as GridNode;
            GridNode nodeInFront = null;

            if (actorNode == AdjacentCells[0].RelativeNode && AdjacentCells[1] != null)
                nodeInFront = AdjacentCells[1].RelativeNode;
            else
                nodeInFront = AdjacentCells[0].RelativeNode;

            Room nextRoom = null;
            if (nodeInFront != null)
            {
                //Se trova una stanza la collega e aggiorna lo stato delle stanze/porte nel dungeon
                nextRoom = nodeInFront.RelativeCell.RelativeRoom;
                if (nextRoom.StatusOfExploration == ExplorationStatus.NotExplored)
                {
                    GameManager.I.DungeonMng.UpdateRoomStatus(nextRoom, ExplorationStatus.InExploration);
                }
            }
            else
            {
                //Se non trova nulla, semplicemente aggiunge solo il link della cella davanti
                AddLinks(new List<INetworkable>() { nodeInFront });
            }

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

