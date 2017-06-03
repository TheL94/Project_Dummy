using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRoomController : MonoBehaviour, IPointerClickHandler, IDragHandler {

    public Room ActualRoom;

    public void OnDrag(PointerEventData eventData)
    {
        ActualRoom.transform.position = Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!eventData.dragging)
            ActualRoom.Rotate();
    }
}
