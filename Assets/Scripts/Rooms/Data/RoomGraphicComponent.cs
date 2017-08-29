using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Rooms
{
    [CreateAssetMenu(fileName = "RoomGraphicComponent", menuName = "Room/NewRoomGraphicComponent", order = 2)]
    public class RoomGraphicComponent : ScriptableObject
    {
        public string ID;
        public GameObject ObjPrefab;
        public int NumberOfObj;
    }
}