using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Rooms
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "Room/NewRoomData", order = 1)]
    public class RoomData : ScriptableObject
    {
        public bool SelfTrimming;
        public bool SelfDoorTrim;
        public int MinNumberOfCells;
        public float PenetrationOffset;
        public float RoomExpansionPercentageDecay;
        public float DoorNumberPercentage;
        public RoomElements RoomElements;
    }
}