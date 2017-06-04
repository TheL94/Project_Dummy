using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRoomController : MonoBehaviour, IPointerClickHandler, IDragHandler, IDropHandler {

    public Room ActualRoom;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!eventData.dragging)
            ActualRoom.Rotate();            
    }

    public void OnDrag(PointerEventData eventData)
    {
        ActualRoom.RoomMovment.DragActions(eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("drop");
        ActualRoom.RoomMovment.DropActions(eventData);
    }
}
