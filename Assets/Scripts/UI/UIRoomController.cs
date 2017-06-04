using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRoomController : MonoBehaviour, IPointerClickHandler, IDragHandler, IDropHandler, IPointerDownHandler {

    public Room ActualRoom;

    public void OnDrag(PointerEventData eventData)
    {
        ActualRoom.RoomMovment.FollowMousePosition(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!eventData.dragging)
            ActualRoom.Rotate();            
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("drop");
        ActualRoom.RoomMovment.DropAction(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ActualRoom.StartPosition = ActualRoom.transform.position;
    }
}
