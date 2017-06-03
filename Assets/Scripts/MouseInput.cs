using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInput : MonoBehaviour, IDragHandler, IDropHandler {

    Room room;

    public void Init(Room _room)
    {
        room = _room;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("drag");
        GridNode node = GameManager.I.GridCtrl.GetSpecificGridNode(Camera.main.ScreenToWorldPoint(eventData.position));

        if (node != null)
            transform.position = node.WorldPosition;
        else
            transform.position = Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("drop");

        GridNode node = GameManager.I.GridCtrl.GetSpecificGridNode(Camera.main.ScreenToWorldPoint(eventData.position));

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
