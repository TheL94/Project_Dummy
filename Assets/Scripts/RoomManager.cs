using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {

    public Cell RoomPrefab;

    public List<Transform> SpanwPoints = new List<Transform>();

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

    public void InstantiateRoom(Vector3 _spawn)
    {
        Room tempRoom = Instantiate(RoomPrefab, _spawn, Quaternion.identity).GetComponent<Room>();
        tempRoom.Init(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InstantiateRoom(SpanwPoints[0].position);
        }
    }
}
