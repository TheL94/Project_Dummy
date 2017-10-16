using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DumbProject.Rooms;
using DumbProject.Generic;

namespace DumbProject.UI
{
    // TODO : da rivedere quando rifaremo la UI
    public class UIRoomController : MonoBehaviour /*, IPointerClickHandler, IDragHandler, IPointerUpHandler, IDropHandler*/
    {
        [HideInInspector]
        public Room ActualRoom;

        public Vector2 AnchorMin { get { return (transform as RectTransform).anchorMin; } }
        public Vector2 AnchorMax { get { return (transform as RectTransform).anchorMax; } }

        public bool CheckIfInputIsForRoomPreview(Vector2 _position)
        {
            Vector2 deviceResolution = GameManager.I.UIMng.CurrentResolution;

            if (GameManager.I.DeviceEnvironment == DeviceType.Desktop)
            {
                if (GameManager.I.UIMng.ForceVerticalUI)
                    return CheckPortraitPosition(_position, deviceResolution);
                else
                    return CheckLandscapePosition(_position, deviceResolution);
            }
            else
            {
                if (GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.Portrait || GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.PortraitUpsideDown)
                    return CheckPortraitPosition(_position, deviceResolution);
                else
                    return CheckLandscapePosition(_position, deviceResolution);
            }

        }

        bool CheckPortraitPosition(Vector2 _position, Vector2 _deviceResolution)
        {
            float Xmax = AnchorMax.x * _deviceResolution.y;
            float Xmin = AnchorMin.x * _deviceResolution.y;
            float Ymin = AnchorMin.y * _deviceResolution.x;
            float Ymax = AnchorMax.y * _deviceResolution.x;

            if (_position.x < Xmax && _position.x > Xmin && _position.y < Ymax && _position.y > Ymin)
                return true;
            else
                return false;
        }

        bool CheckLandscapePosition(Vector2 _position, Vector2 _deviceResolution)
        {
            float Xmin = AnchorMin.x * _deviceResolution.x;
            float Xmax = AnchorMax.x * _deviceResolution.x;
            float Ymin = AnchorMin.y * _deviceResolution.y;
            float Ymax = AnchorMax.y * _deviceResolution.y;

            if (_position.x < Xmax && _position.x > Xmin &&  _position.y < Ymax && _position.y > Ymin)
                return true;
            else
                return false;
        }

        public void CallRotateRoom()
        {
            if (GameManager.I.CurrentState != Flow.FlowState.Gameplay)
                return;
            if (ActualRoom != null)
                ActualRoom.RoomMovment.RotateClockwise();
        }

        public void OnDrag(Vector2 _mousePosition)
        {
            if (GameManager.I.CurrentState != Flow.FlowState.Gameplay)
                return;
            if (ActualRoom != null)
                ActualRoom.RoomMovment.DragActions(_mousePosition);
        }

        public void CallPlaceRoom()
        {
            if (GameManager.I.CurrentState != Flow.FlowState.Gameplay)
                return;
            if (ActualRoom != null)
            {
                if (ActualRoom.RoomMovment.DropActions())
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