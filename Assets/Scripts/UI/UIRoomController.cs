using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DumbProject.Rooms;

namespace DumbProject.UI
{
    public class UIRoomController : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerUpHandler, IDropHandler
    {
        [HideInInspector]
        public Room ActualRoom;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!eventData.dragging && ActualRoom != null)
                ActualRoom.RotateClockwise();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (ActualRoom != null)
                ActualRoom.RoomMovment.DragActions(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.dragging && ActualRoom != null)
            {
                if (ActualRoom.RoomMovment.DropActions(eventData))
                {
                    GameManager.I.RoomGenerator.ReleaseRoomSpawn(ActualRoom);
                    GameManager.I.RoomGenerator.CreateNewRoom();
                }
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("DropAction");
        }
    }
}