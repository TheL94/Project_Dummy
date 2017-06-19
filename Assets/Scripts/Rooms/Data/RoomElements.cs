using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Rooms.Data
{
    // TODO : da cancellare, attualmente non serve a niente
    [CreateAssetMenu(fileName = "RoomElements", menuName = "Room/NewRoomElements", order = 2)]
    public class RoomElements : ScriptableObject
    {
        public GameObject FloorPrefab;
        public GameObject WallPrefab;
        public GameObject PillarPrefab;
        public GameObject ArchPrefab;
        public GameObject DoorPrefab;
    }
}