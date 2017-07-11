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
                    if(edge.Type == EdgeType.Door && edge.StatusOfExploration == ExplorationStatus.NotExplored)
                        unexploredDoors.Add(edge);
                }
            }
            return unexploredDoors;
        }

        /// <summary>
        /// Regole per settare lo stato di esplorazione delle stanze
        /// </summary>
        /// <param name="_room"></param>
        public void SetDoorsStatus(Room _room)
        {
            List<Edge> edges = _room.GetListOfEdges().Where(e => e.Type == EdgeType.Door).ToList();
            GridNode node = null;
            foreach (Edge edge in edges)
            {
                node = edge.RelativeCell.RelativeNode.RelativeGrid.GetSpecificGridNode(edge.GetFrontPosition());
                if (node != null && node.RelativeCell != null)
                {
                    Room roomInFront = node.RelativeCell.RelativeRoom;
                    switch (_room.StatusOfExploration)
                    {
                        case ExplorationStatus.NotInGame:
                            edge.StatusOfExploration = ExplorationStatus.NotInGame;
                            break;
                        case ExplorationStatus.Unavailable:
                            switch (roomInFront.StatusOfExploration)
                            {
                                case ExplorationStatus.NotInGame:
                                    roomInFront.StatusOfExploration = ExplorationStatus.Unavailable;
                                    break;
                                case ExplorationStatus.Unavailable:
                                case ExplorationStatus.NotExplored:
                                    edge.StatusOfExploration = ExplorationStatus.Unavailable;
                                    break;
                                case ExplorationStatus.InExploration:
                                case ExplorationStatus.Explored:
                                    _room.StatusOfExploration = ExplorationStatus.NotExplored;
                                    break;
                            }
                            break;
                        case ExplorationStatus.NotExplored:
                            switch (roomInFront.StatusOfExploration)
                            {
                                case ExplorationStatus.NotInGame:
                                    roomInFront.StatusOfExploration = ExplorationStatus.Unavailable;
                                    break;
                                case ExplorationStatus.Unavailable:
                                case ExplorationStatus.NotExplored:
                                    edge.StatusOfExploration = ExplorationStatus.Unavailable;
                                    break;
                                case ExplorationStatus.InExploration:
                                case ExplorationStatus.Explored:
                                    edge.StatusOfExploration = ExplorationStatus.NotExplored;
                                    break;
                            }
                            break;
                        case ExplorationStatus.InExploration:
                            switch (roomInFront.StatusOfExploration)
                            {
                                case ExplorationStatus.NotInGame:
                                    roomInFront.StatusOfExploration = ExplorationStatus.NotExplored;
                                    break;
                                case ExplorationStatus.Unavailable:
                                    roomInFront.StatusOfExploration = ExplorationStatus.NotExplored;
                                    break;
                                case ExplorationStatus.NotExplored:
                                    edge.StatusOfExploration = ExplorationStatus.NotExplored;
                                    break;
                                case ExplorationStatus.InExploration:
                                    Debug.LogWarning("Due stanze in esplorazione contemporaneamente !");
                                    break;
                                case ExplorationStatus.Explored:
                                    edge.StatusOfExploration = ExplorationStatus.Explored;
                                    break;
                            }
                            break;
                        case ExplorationStatus.Explored:
                            switch (roomInFront.StatusOfExploration)
                            {
                                case ExplorationStatus.NotInGame:
                                    roomInFront.StatusOfExploration = ExplorationStatus.NotExplored;
                                    break;
                                case ExplorationStatus.Unavailable:
                                    roomInFront.StatusOfExploration = ExplorationStatus.NotExplored;
                                    break;
                                case ExplorationStatus.NotExplored:
                                    edge.StatusOfExploration = ExplorationStatus.NotExplored;
                                    break;
                                case ExplorationStatus.InExploration:
                                case ExplorationStatus.Explored:
                                    edge.StatusOfExploration = ExplorationStatus.Explored;
                                    break;
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
                        case ExplorationStatus.NotExplored:
                            edge.StatusOfExploration = ExplorationStatus.Unavailable;
                            break;
                        case ExplorationStatus.InExploration:
                        case ExplorationStatus.Explored:
                            edge.StatusOfExploration = ExplorationStatus.NotExplored;
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