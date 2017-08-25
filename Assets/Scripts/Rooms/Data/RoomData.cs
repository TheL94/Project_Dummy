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
        [Range(1, 9)]
        public int MinNumberOfCells;
        public float PenetrationOffset;
        [Range(0f, 1f)]
        public float RoomExpansionPercentageDecay;
        [Range(0f, 4f)]
        public float DoorNumberPercentage;
    }
}