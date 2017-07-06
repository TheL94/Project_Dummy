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
            _room.StatusOfExploration = _status;
            UpdateRoomConnections();
        }

        public List<Edge> GetAllUnexploredDoors()
        {
            List<Edge> unexploredDoors = new List<Edge>();
            foreach (Room room in RoomInDungeon)
            {
                foreach (Edge edge in room.GetListOfEdges())
                {
                    if(edge.Type == EdgeType.Door && edge.StatusOfExploration == ExplorationStatus.Unexplored)
                        unexploredDoors.Add(edge);
                }
            }
            return unexploredDoors;
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
                    switch (_room.StatusOfExploration)
                    {
                        case ExplorationStatus.NotInGame:
                            edge.StatusOfExploration = ExplorationStatus.NotInGame;
                            break;
                        case ExplorationStatus.Unavailable:
                            if (node.RelativeCell.RelativeRoom.StatusOfExploration == ExplorationStatus.Unavailable)
                            {
                                edge.StatusOfExploration = ExplorationStatus.Unavailable;
                            }
                            else if (node.RelativeCell.RelativeRoom.StatusOfExploration == ExplorationStatus.Unexplored)
                            {
                                edge.StatusOfExploration = ExplorationStatus.Unavailable;
                            }
                            else if (node.RelativeCell.RelativeRoom.StatusOfExploration == ExplorationStatus.Explored)
                            {
                                edge.StatusOfExploration = ExplorationStatus.Unexplored;
                                _room.StatusOfExploration = ExplorationStatus.Unexplored;
                            }
                            break;
                        case ExplorationStatus.Unexplored:
                            if (node.RelativeCell.RelativeRoom.StatusOfExploration == ExplorationStatus.Unavailable)
                            {
                                edge.StatusOfExploration = ExplorationStatus.Unavailable;
                            }
                            else if (node.RelativeCell.RelativeRoom.StatusOfExploration == ExplorationStatus.Unexplored)
                            {
                                edge.StatusOfExploration = ExplorationStatus.Unavailable;
                            }
                            else if (node.RelativeCell.RelativeRoom.StatusOfExploration == ExplorationStatus.Explored)
                            {
                                edge.StatusOfExploration = ExplorationStatus.Unexplored;
                            }
                            break;
                        case ExplorationStatus.Explored:
                            if (node.RelativeCell.RelativeRoom.StatusOfExploration == ExplorationStatus.Unavailable)
                            {
                                edge.StatusOfExploration = ExplorationStatus.Unexplored;
                                node.RelativeCell.RelativeRoom.StatusOfExploration = ExplorationStatus.Unexplored;
                            }
                            else if (node.RelativeCell.RelativeRoom.StatusOfExploration == ExplorationStatus.Unexplored)
                            {
                                edge.StatusOfExploration = ExplorationStatus.Unexplored;
                            }
                            else if (node.RelativeCell.RelativeRoom.StatusOfExploration == ExplorationStatus.Explored)
                            {
                                edge.StatusOfExploration = ExplorationStatus.Explored;
                            }
                            break;
                    }
                }
                else if (node != null && node.RelativeCell == null)
                {
                    switch (_room.StatusOfExploration)
                    {
                        case ExplorationStatus.NotInGame:
                            edge.StatusOfExploration = ExplorationStatus.NotInGame;
                            break;
                        case ExplorationStatus.Unavailable:
                        case ExplorationStatus.Unexplored:
                            edge.StatusOfExploration = ExplorationStatus.Unavailable;
                            break;
                        case ExplorationStatus.Explored:
                            edge.StatusOfExploration = ExplorationStatus.Unexplored;
                            break;
                    }
                }
            }
        }
        #endregion
        #endregion

        void UpdateRoomConnections()
        {
            foreach (Room room in RoomInDungeon)
            {
                room.LinkCells();
            }
        }
    }
}