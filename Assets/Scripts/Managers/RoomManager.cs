using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Room RoomPrefab;
    [HideInInspector]
    public Room FirstRoom;

    public void Setup()
    {
        InstantiateFirstRoom(RoomPrefab, GameManager.I.GridCtrl.GetGridCenter());
    }
    
    void InstantiateFirstRoom(Room _firstRoomPrefab, GridNode _node)
    {
        FirstRoom = Instantiate(_firstRoomPrefab, _node.WorldPosition, Quaternion.identity);
        _node.RelativeCell = FirstRoom.RoomCells[0];
        FirstRoom.GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
        FirstRoom.name = "MainRoom";
    }
}
