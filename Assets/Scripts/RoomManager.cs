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
        for (int i = 0; i < SpanwPoints.Count; i++)
        {
            InstantiateRoom(SpanwPoints[i].position);
        }
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
        int randNum = Random.Range(1, 5);
        switch (randNum)
        {
            case 1:
                tempRoom.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case 2:
                tempRoom.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case 3:
                tempRoom.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case 4:
                tempRoom.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            default:
                tempRoom.GetComponent<SpriteRenderer>().color = Color.black;
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InstantiateRoom(SpanwPoints[0].position);
        }
    }
}
