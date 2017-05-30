using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {

    public Cell RoomPrefab;

    Cell firstRoom;

    public void Setup()
    {
       InstantiateFirstRoom(RoomPrefab, GameManager.I.GridCtrl.GetGridCenter());
    }

    public void InstantiateFirstRoom(Cell _roomPrefab , GridNode _node)
    {
        firstRoom = Instantiate(_roomPrefab, _node.WorldPosition, Quaternion.identity);
        _node.RelativeCell = firstRoom;
        firstRoom.GetComponent<SpriteRenderer>().color = Color.yellow;
        firstRoom.name = "MainRoom";   
    }
}
