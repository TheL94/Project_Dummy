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
        Vector3 objPosition = new Vector3(Camera.main.ScreenToWorldPoint(_eventData.position).x, 0, Camera.main.ScreenToWorldPoint(_eventData.position).z);
        GridNode node = GameManager.I.GridCtrl.GetSpecificGridNode(objPosition);

        if (node != null)
            transform.position = node.WorldPosition;
        else
            transform.position = objPosition;
    }

    public void DropAction(PointerEventData _eventData)
    {
        Vector3 objPosition = new Vector3(Camera.main.ScreenToWorldPoint(_eventData.position).x, 0, Camera.main.ScreenToWorldPoint(_eventData.position).z);
        GridNode node = GameManager.I.GridCtrl.GetSpecificGridNode(objPosition);

        if (node != null && node.RelativeCell == null)
        {
            if (GameManager.I.GridCtrl.CheckAdjacentNodesRelativeCell(node))
            {
                transform.position = node.WorldPosition;
                node.RelativeCell = room.RoomCells[0];
                return;
            }
        }

        room.transform.position = room.StartPosition;
    }
}
