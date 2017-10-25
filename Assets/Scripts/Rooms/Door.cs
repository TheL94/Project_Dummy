using DumbProject.Generic;
using DumbProject.Grid;
using Framework.Pathfinding;
using Framework.AI;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Rooms
{
    public class Door : Edge, IInteractable
    {
        public ExplorationStatus StatusOfExploration
        {
            get { return EvaluateStatus(); }
        }

        NetNode _relativeNetNode;
        public NetNode RelativeNetNode
        {
            get { return _relativeNetNode; }
            set { _relativeNetNode = value; }
        }

        Cell[] _adjacentCells = new Cell[2];
        public Cell[] AdjacentCells { get { return _adjacentCells; } }

        DoorIndicator indicator;

        #region API
        public override void Setup(Cell _relativeCell)
        {
            if (gameObject.GetComponents<Door>().Length > 1)
            {
                Debug.LogWarning("Due Door ! " + transform.position);
                DestroyImmediate(this);
            }

            base.Setup(_relativeCell);
            AddAjdacentCell(RelativeCell);

            indicator = gameObject.AddComponent<DoorIndicator>();
            indicator.Init(this);
        }

        public override void DisableAllGraphics()
        {
            if(indicator != null)
                indicator.DisableGraphic();
            base.DisableAllGraphics();
        }

        public override void DestroyObject(bool _destroyComponentOnly = false)
        {
            RelativeCell.Doors.Remove(this);
            base.DestroyObject(_destroyComponentOnly);
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
            if (RelativeNetNode == null)
                return;
            for (int i = 0; i < AdjacentCells.Length; i++)
            {
                if (AdjacentCells[i] == null)
                    continue;

                if(AdjacentCells[i].RelativeRoom.StatusOfExploration == ExplorationStatus.Explored || AdjacentCells[i].RelativeRoom.StatusOfExploration == ExplorationStatus.InExploration)
                {
                    INetworkable networkable = AdjacentCells[i].RelativeNode as INetworkable;
                    RelativeNetNode.AddLinks(new List<INetworkable>() { networkable });
                    AdjacentCells[i].RelativeNode.AddLinks(new List<INetworkable>() { RelativeNetNode });
                }
            }
        }

        //Implementazione di IInteractable
        #region IInteractable
        public Transform Transf { get { return transform; } }
        bool _isInteractable = true;
        public bool IsInteractable
        {
            get
            {
                if (StatusOfExploration == ExplorationStatus.Explored)
                    _isInteractable = false;
                return _isInteractable;
            }
            set { _isInteractable = value; }
        }

        public void Interact(AI_Controller _controller)
        {
            foreach (Cell cell in AdjacentCells)
            {
                if (cell != null)
                {
                    RelativeNetNode.AddLinks(new List<INetworkable>() { cell.RelativeNode });
                    cell.RelativeNode.AddLinks(new List<INetworkable>() { RelativeNetNode });
                    //Se la cella appartiene ad una stanza esplorata non modifica lo status, altrimenti porta lo stato della nuova stanza in esplorazione
                    ExplorationStatus statusToSet = cell.RelativeRoom.StatusOfExploration == ExplorationStatus.Explored ? ExplorationStatus.Explored : ExplorationStatus.InExploration;
                    if (statusToSet != ExplorationStatus.Explored)
                        GameManager.I.DungeonMng.UpdateRoomStatus(cell.RelativeRoom, statusToSet);
                }
                else
                {
                    //Caso in cui la porta collega all'esterno
                    //RelativeNetNode.AddLinks(new List<INetworkable>() { RelativeCell.RelativeNode.RelativeGrid.GetSpecificGridNode(GetOppositeOfRelativeCellPosition()) });
                    _controller.SetAICurrentState((_controller as Dumby).DefaultLostCondition);
                }
            }

            //se uno dei nodi collegati della porta è quello in cui c'è l'actor prende l'altro
            //WARNING: stiamo assumendo che Links[0] sia sempre il primo nodo e che sia quindi quello da cui Actor arriva
            //_controller.INetworkableObjective = RelativeNetNode.Links[1];
            IsInteractable = false;
        }
        #endregion
        #endregion

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
    }
}

