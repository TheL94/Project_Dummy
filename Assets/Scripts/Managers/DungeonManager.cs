using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DumbProject.Rooms;
using DumbProject.Grid;

namespace DumbProject.Generic
{
    public class DungeonManager : MonoBehaviour
    {
        [HideInInspector]
        public DropController DropCtrl;
        [HideInInspector]
        public List<Room> RoomInDungeon = new List<Room>();

        #region API
        public void Setup()
        {
            DropCtrl = new DropController();
        }

        public void Clean()
        {
            foreach (Room room in RoomInDungeon)
                Destroy(room.gameObject);
        }

        #region Rooms
        public void ParentRoom(Room _room, ExplorationStatus _status = ExplorationStatus.Unavailable)
        {
            _room.transform.parent = transform;
            RoomInDungeon.Add(_room);
            _room.Status = _status;
            UpdateRoomConnections();
        }
        

        void UpdateRoomConnections()
        {
            foreach (Room room in RoomInDungeon)
            {
                room.LinkCells();
            }
        }

        /// <summary>
        /// Regole per settare lo stato di esplorazione delle stanze
        /// </summary>
        /// <param name="_room"></param>
        public void SetRoomStausBasedOnCloseRoomsStatus(Room _room)
        {
            List<Edge> edges = _room.GetListOfEdges().Where(e => e.Type == EdgeType.Door).ToList();
            GridNode node = null;
            foreach (Edge edge in edges)
            {
                node = edge.RelativeCell.RelativeNode.RelativeGrid.GetSpecificGridNode(edge.GetNodeInFrontPosition());
                if (node != null && node.RelativeCell != null)
                {
                    switch (_room.Status)
                    {
                        case ExplorationStatus.NotInGame:
                            edge.Status = ExplorationStatus.NotInGame;
                            break;
                        case ExplorationStatus.Unavailable:
                            if (node.RelativeCell.RelativeRoom.Status == ExplorationStatus.Unavailable)
                            {
                                edge.Status = ExplorationStatus.Unavailable;
                            }
                            else if (node.RelativeCell.RelativeRoom.Status == ExplorationStatus.Unexplored)
                            {
                                edge.Status = ExplorationStatus.Unavailable;
                            }
                            else if (node.RelativeCell.RelativeRoom.Status == ExplorationStatus.Explored)
                            {
                                edge.Status = ExplorationStatus.Unexplored;
                                _room.Status = ExplorationStatus.Unexplored;
                            }
                            break;
                        case ExplorationStatus.Unexplored:
                            if (node.RelativeCell.RelativeRoom.Status == ExplorationStatus.Unavailable)
                            {
                                edge.Status = ExplorationStatus.Unavailable;
                            }
                            else if (node.RelativeCell.RelativeRoom.Status == ExplorationStatus.Unexplored)
                            {
                                edge.Status = ExplorationStatus.Unavailable;
                            }
                            else if (node.RelativeCell.RelativeRoom.Status == ExplorationStatus.Explored)
                            {
                                edge.Status = ExplorationStatus.Unexplored;
                            }
                            break;
                        case ExplorationStatus.Explored:
                            if (node.RelativeCell.RelativeRoom.Status == ExplorationStatus.Unavailable)
                            {
                                edge.Status = ExplorationStatus.Unexplored;
                                node.RelativeCell.RelativeRoom.Status = ExplorationStatus.Unexplored;
                            }
                            else if (node.RelativeCell.RelativeRoom.Status == ExplorationStatus.Unexplored)
                            {
                                edge.Status = ExplorationStatus.Unexplored;
                            }
                            else if (node.RelativeCell.RelativeRoom.Status == ExplorationStatus.Explored)
                            {
                                edge.Status = ExplorationStatus.Explored;
                            }
                            break;
                    }
                }
                else if (node != null && node.RelativeCell == null)
                {
                    switch (_room.Status)
                    {
                        case ExplorationStatus.NotInGame:
                            edge.Status = ExplorationStatus.NotInGame;
                            break;
                        case ExplorationStatus.Unavailable:
                        case ExplorationStatus.Unexplored:
                            edge.Status = ExplorationStatus.Unavailable;
                            break;
                        case ExplorationStatus.Explored:
                            edge.Status = ExplorationStatus.Unexplored;
                            break;
                    }
                }
            }
        }
        #endregion
        #endregion
    }
}