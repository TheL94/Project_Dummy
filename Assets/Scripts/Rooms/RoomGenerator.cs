using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;

namespace DumbProject.Rooms
{
    public class RoomGenerator : MonoBehaviour
    {
        public List<Room> RoomTypes = new List<Room>();
        [HideInInspector]
        public Room FirstRoom;

        public void Setup()
        {

        }
    }
}