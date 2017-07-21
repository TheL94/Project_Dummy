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
            if (gameObject.GetComponents<Door>().Length > 1)
            {
                Debug.LogWarning("Due Door ! " + transform.position);
                DestroyImmediate(this);
            }

            base.Setup(_relativeCell);
            AddAjdacentCell(RelativeCell);
        }

        /// <summary>
        /// Funzione che disabilita l'oggetto
        /// </summary>
        /// <param name="_avoidDestruction"></param>
        public override void DisableObject(bool _avoidDestruction = false)
        {
            DisableGraphic();
            RelativeCell.Doors.Remove(this);
            RelativeCell.RelativeNode.Links.Remove(this);
            Debug.Log("Door spenta");

            if (!_avoidDestruction)
                Destroy(gameObject);
            else
                Destroy(this);
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
        public List<INetworkable> Links {
            get { return _links; }
            set { _links = value; } }

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
        public bool IsInteractable {
            get
            {
                if (StatusOfExploration == ExplorationStatus.Explored)
                    _isInteractable = false;
                return _isInteractable;
            }
            set { _isInteractable = value; }
        }

        public void Interact(AIActor _actor)
        {
            foreach (Cell cell in AdjacentCells)
            {
                if(cell != null)
                {
                    AddLinks(new List<INetworkable>() { cell.RelativeNode });
                    cell.RelativeNode.AddLinks(new List<INetworkable>() { this });
                    //Se la cella appartiene ad una stanza esplorata non modifica lo status, altrimenti porta lo stato della nuova stanza in esplorazione
                    ExplorationStatus statusToSet = cell.RelativeRoom.StatusOfExploration == ExplorationStatus.Explored ? ExplorationStatus.Explored : ExplorationStatus.InExploration;
                    if(statusToSet != ExplorationStatus.Explored)
                        GameManager.I.DungeonMng.UpdateRoomStatus(cell.RelativeRoom, statusToSet);
                }
                else
                {
                    //Caso in cui la porta collega all'esterno
                    AddLinks(new List<INetworkable>() { RelativeCell.RelativeNode.RelativeGrid.GetSpecificGridNode(GetOppositeOfRelativeCellPosition()) });
                }
            }

            //se uno dei nodi collegati della porta è quello in cui c'è l'actor prende l'altro
            //WARNING: stiamo assumendo che Links[0] sia sempre il primo nodo e che sia quindi quello da cui Actor arriva
            _actor.INetworkableObjective = Links[1];
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
        }
    }
}

