using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomMovment : MonoBehaviour {

    Room room;

    public void Init(Room _room)
    {
        room = _room;
    }

    public void FollowMousePosition(PointerEventData _eventData)
    {
        //Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100);
        //Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //GridNode node = GameManager.I.GridCtrl.GetSpecificGridNode(objPosition);

        //if (node != null)
        //    transform.position = node.WorldPosition;
        //else
        //    transform.position = objPosition;

        GridNode node = GameManager.I.GridCtrl.GetSpecificGridNode(new Vector3(Camera.main.ScreenToWorldPoint(_eventData.position).x, 0, Camera.main.ScreenToWorldPoint(_eventData.position).z));

        if (node != null)
            transform.position = node.WorldPosition;
        else
            transform.position = Camera.main.ScreenToWorldPoint(_eventData.position);
    }

    public void DropAction(PointerEventData _eventData)
    {
        Debug.Log("drop");
        GridNode node = GameManager.I.GridCtrl.GetSpecificGridNode(new Vector3(Camera.main.ScreenToWorldPoint(_eventData.position).x, 0, Camera.main.ScreenToWorldPoint(_eventData.position).z));

        if (node != null & node.RelativeCell == null)
        {
            if (GameManager.I.GridCtrl.CheckAdjacentNodesRelativeCell(node))
            {
                transform.position = node.WorldPosition;
                node.RelativeCell = GetComponent<Cell>();
                return;
            }
        }
        transform.position = room.StartPosition;
    }
}
