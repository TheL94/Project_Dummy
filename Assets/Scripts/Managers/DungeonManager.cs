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
        public void ParentRoom(Room _room, ExplorationStatus _status)
        {
            _room.transform.parent = transform;
            if (!RoomInDungeon.Contains(_room))
                RoomInDungeon.Add(_room);
            UpdateRoomStatus(_room, _status);
        }

        public void ParentRoom(Room _room)
        {
            if (!RoomInDungeon.Contains(_room))
                RoomInDungeon.Add(_room);
            ParentRoom(_room, EvaluateRoomStatus(_room, GetAdjacentRoomsByDoors(_room)));
        }

        public List<Door> GetAllUnexploredDoors()
        {
            List<Door> unexploredDoors = new List<Door>();
            foreach (Room room in RoomInDungeon)
            {
                foreach (Door door in room.GetListOfDoors())
                {
                    if(door.StatusOfExploration == ExplorationStatus.NotExplored)
                        unexploredDoors.Add(door);
                }
            }
            return unexploredDoors;
        }

        public List<Room> GetAdjacentRoomsByDoors(Room _objective)
        {
            List<Room> roomToReturn = new List<Room>();

            foreach (Room room in RoomInDungeon)
            {
                List<Door> doors = room.GetListOfDoors();
                foreach (Door door in doors)
                {
                    for (int i = 0; i < door.AdjacentCells.Length; i++)
                    {
                        if (door.AdjacentCells[i] != null && door.AdjacentCells[i].RelativeRoom == _objective)
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

        /// <summary>
        /// Fuinzione che valuta lo stato di default della stanza
        /// </summary>
        /// <param name="_placedRoom"></param>
        /// <returns></returns>
        ExplorationStatus EvaluateRoomStatus(Room _placedRoom, List<Room> _adjRooms)
        {
            if (_placedRoom.StatusOfExploration != ExplorationStatus.Unavailable && _placedRoom.StatusOfExploration != ExplorationStatus.NotInGame)
                return _placedRoom.StatusOfExploration;

            ExplorationStatus returnStatus = ExplorationStatus.NotInGame;

            foreach (Room room in _adjRooms)
            {
                if (room.StatusOfExploration == ExplorationStatus.Explored || room.StatusOfExploration == ExplorationStatus.InExploration)
                    return ExplorationStatus.NotExplored;
                else if (room.StatusOfExploration == ExplorationStatus.NotExplored || room.StatusOfExploration == ExplorationStatus.Unavailable)
                    returnStatus = ExplorationStatus.Unavailable;
            }

            return returnStatus;
        }

        /// <summary>
        /// Update the exploration status of a given room to a new given Status
        /// </summary>
        /// <param name="_room"></param>
        /// <param name="_newStatus"></param>
        public void UpdateRoomStatus(Room _room, ExplorationStatus _newStatus)
        {
            _room.StatusOfExploration = _newStatus;
            _room.LinkCells();

            List<Room> adjRooms = GetAdjacentRoomsByDoors(_room);
            foreach (Room room in adjRooms)
            {
                room.StatusOfExploration = EvaluateRoomStatus(room, GetAdjacentRoomsByDoors(room));
                room.LinkCells();
            }
        }
    }
}