using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms.Data;

namespace DumbProject.Rooms
{
    public class RoomGenerator : MonoBehaviour
    {
        public List<RoomData> RoomTypesData = new List<RoomData>();

        List<RoomData> RoomTypesInstances = new List<RoomData>();

        public void Setup()
        {
            foreach (RoomData roomData in RoomTypesData)
            {
                RoomTypesInstances.Add(Instantiate(roomData));
            }

            NewRoom();
        }

        void NewRoom()
        {
            foreach (RoomData roomData in RoomTypesData)
            {
                if (roomData.Shape == RoomShape.T_Shape)
                    InstantiateRoom(roomData);
            }
        }

        void InstantiateRoom(RoomData _data)
        {
            Room newRoom = Instantiate(_data.RoomPrefab);
            newRoom.Setup(_data, GameManager.I.RoomPreviewCtrl.GetFirstGridAvailable());
        }
    }
}