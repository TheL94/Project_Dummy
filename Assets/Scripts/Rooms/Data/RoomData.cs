using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms.Cells;

namespace DumbProject.Rooms.Data
{
    // TODO : da cancellare, attualmente non serve a niente
    [CreateAssetMenu(fileName = "RoomData", menuName = "Room/NewRoom", order = 1)]
    public class RoomData : ScriptableObject
    {
        public RoomShape Shape;
        public float WallPenetrationOffset;
        public Cell CellPrefab;
    }
}