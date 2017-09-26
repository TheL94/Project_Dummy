﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DumbProject.Rooms;
using DumbProject.Generic;

namespace DumbProject.UI
{
    // TODO : da rivedere quando rifaremo la UI
    public class UIRoomController : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerUpHandler/* IDropHandler*/
    {
        [HideInInspector]
        public Room ActualRoom;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (GameManager.I.CurrentState != Flow.FlowState.Gameplay)
                return;
            if (!eventData.dragging && ActualRoom != null)
                ActualRoom.RoomMovment.RotateClockwise();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (GameManager.I.CurrentState != Flow.FlowState.Gameplay)
                return;
            if (eventData.dragging && ActualRoom != null)
                ActualRoom.RoomMovment.DragActions(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (GameManager.I.CurrentState != Flow.FlowState.Gameplay)
                return;
            if (eventData.dragging && ActualRoom != null)
            {
                if (ActualRoom.RoomMovment.DropActions(eventData))
                {
                    GameManager.I.RoomGenerator.ReleaseRoomSpawn(ActualRoom);
                    GameManager.I.RoomGenerator.CreateNewRoom();
                }
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            if (!focus)
                ActualRoom.RoomMovment.MovingToInitialPosition = true;
        }

        //TODO: metti apposto ALberto
        //public void OnDrop(PointerEventData eventData)
        //{
        //    if (eventData.pointerDrag.GetComponent<IventoryItem>() != null)
        //    {
        //        if (tempCell != null)
        //        {
        //            PlaceItemInRoom(eventData.pointerDrag.GetComponent<IventoryItem>().ItemToInstantiate, tempCell);
        //            eventData.pointerDrag.GetComponent<IventoryItem>().DestroyObj();
        //        }
        //        else 
        //            eventData.pointerDrag.GetComponent<IventoryItem>().PlaceInStartPosition();
        //    }
        //}
    }
}