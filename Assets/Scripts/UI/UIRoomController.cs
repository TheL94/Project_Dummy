using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DumbProject.Rooms;

namespace DumbProject.UI
{
    public class UIRoomController : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerUpHandler
    {
        [HideInInspector]
        public Room ActualRoom;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!eventData.dragging)
                ActualRoom.Rotate();
        }

        public void OnDrag(PointerEventData eventData)
        {
            ActualRoom.ReleaseCell();
            ActualRoom.RoomMovment.DragActions(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.dragging)
            {
                if (ActualRoom.RoomMovment.DropActions(eventData))
                {
                    Destroy(ActualRoom.RoomMovment);
                    // crea nuova stanza visualizzata nella ui
                }
            }
        }
    }
}