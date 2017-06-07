using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms.Cells;
using DumbProject.Rooms.Cells.Data;

namespace DumbProject.Rooms.Data
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "Room/NewRoom", order = 1)]
    public class RoomData : ScriptableObject
    {
        public RoomShape Shape;
        public Room RoomPrefab;
        public Cell CellPrefab;
        public CellTypes CellTypes;
    }
}