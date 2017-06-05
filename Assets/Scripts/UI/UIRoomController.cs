using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRoomController : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerUpHandler {

    public Room ActualRoom;
    public int Index;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!eventData.dragging)
            ActualRoom.Rotate();            
    }

    public void OnDrag(PointerEventData eventData)
    {
        ActualRoom.RoomMovment.DragActions(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            if(ActualRoom.RoomMovment.DropActions(eventData))
                ActualRoom = GameManager.I.RoomMng.InstantiateRoom(Index);
        }
    }
}
