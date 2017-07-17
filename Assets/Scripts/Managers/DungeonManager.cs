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

        public List<Door> GetAllUnexploredDoors()
        {
            List<Door> unexploredDoors = new List<Door>();
            foreach (Room room in RoomInDungeon)
            {
                foreach (Door door in room.GetListOfEdges())
                {
                    if(door.StatusOfExploration == ExplorationStatus.NotExplored)
                        unexploredDoors.Add(door);
                }
            }
            return unexploredDoors;
        }

        public List<Room> GetAdjacentRooms(Room _objective)
        {
            List<Room> roomToReturn = new List<Room>();

            foreach (Room room in RoomInDungeon)
            {
                foreach (Door door in room.Doors)
                {
                    for (int i = 0; i < door.AdjacentCells.Length; i++)
                    {
                        if (door.AdjacentCells[i] == _objective)
                        {
                            if(!roomToReturn.Contains(room))
                                roomToReturn.Add(room);
                        }
                    }
                }
            }

            return roomToReturn;
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