using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Rooms
{
    [CreateAssetMenu(fileName = "RoomElements", menuName = "Room/NewRoomElements", order = 2)]
    public class RoomElements : ScriptableObject
    {
        public PoolPrefab Floor;
        public PoolPrefab Wall;
        public PoolPrefab Pillar;
        public PoolPrefab Arch;
    }

    [System.Serializable]
    public struct PoolPrefab
    {
        public GameObject ObjPrefab;
        public int NumberOfObj;
    }
}