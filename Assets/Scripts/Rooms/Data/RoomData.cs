using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms.Cells;

namespace DumbProject.Rooms.Data
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "Room/NewRoom", order = 1)]
    public class RoomData : ScriptableObject
    {
        public float PenetrationOffset;
        public float RoomExpansionPercentageDecay;
        public RoomElements RoomElements;
    }
}