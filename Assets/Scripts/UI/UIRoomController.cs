﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Framework.Rooms;

namespace Framework.UI
{
    public class UIRoomController : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerUpHandler
    {

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
                if (ActualRoom.RoomMovment.DropActions(eventData))
                {
                    Destroy(ActualRoom.RoomMovment);
                    // crea nuova stanza visualizzata nella ui
                    ActualRoom = GameManager.I.RoomGenertor.InstantiateRoom(Index);
                }
            }
        }
    }
}