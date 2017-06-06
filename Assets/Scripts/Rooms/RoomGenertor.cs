using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Grid;

namespace Framework.Rooms
{
    public class RoomGenertor : MonoBehaviour
    {
        public Room RoomPrefab;
        [HideInInspector]
        public Room FirstRoom;

        public void Setup()
        {
            InstantiateFirstRoom(RoomPrefab, GameManager.I.GridCtrl.GetGridCenter());

            for (int i = 0; i < GameManager.I.RoomPreviewCtrl.RoomSpawns.Count; i++)
            {
                GameManager.I.UIMng.roomPreviewController.UIRoomControllers[i].ActualRoom = InstantiateRoom(i);
            }
        }

        void InstantiateFirstRoom(Room _firstRoomPrefab, GridNode _node)
        {
            FirstRoom = Instantiate(_firstRoomPrefab, _node.WorldPosition, Quaternion.identity);
            _node.RelativeCell = FirstRoom.RoomCells[0];
            Destroy(FirstRoom.RoomMovment);
            FirstRoom.GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
            FirstRoom.name = "MainRoom";
        }

        public Room InstantiateRoom(int _roomIndex)
        {
            Room room = Instantiate(RoomPrefab, GameManager.I.RoomPreviewCtrl.RoomSpawns[_roomIndex]);
            room.Init();
            return room;
        }
    }
}