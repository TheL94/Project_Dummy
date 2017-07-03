using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Rooms
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "Room/NewRoom", order = 1)]
    public class RoomData : ScriptableObject
    {
        public float PenetrationOffset;
        public float RoomExpansionPercentageDecay;
        public bool SelfTrimming;
        public RoomElements RoomElements;
    }
}