using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DumbProject.CameraController;
using Multitouch.EventSystems.EventData;
using Multitouch.EventSystems.Gestures;

namespace DumbProject.Generic
{
    public class CameraTouchInput : CameraInput, IPinchHandler, IDragHandler, IPointerDownHandler
    {
        private void Update()
        {
            SetCanvasRect();
        }

        public void OnDrag(PointerEventData eventData)
        {
            camHandler.OnDragEvent(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            camHandler.OnClickEvent(eventData);
        }

        public void OnPinch(SimpleGestures sender, MultiTouchPointerEventData eventData, Vector2 pinchDelta)
        {
            camHandler.PinchToZoom(sender, eventData, pinchDelta);
        }
    }
}