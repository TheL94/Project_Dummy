﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;

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
        public void ParentRoom(Room _room)
        {
            _room.transform.parent = transform;
            RoomInDungeon.Add(_room);
        }
        #endregion
        #endregion
    }
}