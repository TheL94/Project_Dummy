using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DumbProject.CameraController;
using Multitouch.EventSystems.EventData;
using Multitouch.EventSystems.Gestures;

namespace DumbProject.Generic
{
    public class CameraTouchInput : UIBehaviour, IPanHandler, IPinchHandler, IPointerDownHandler
    {
        CameraHandler camMovement;

        new private void Start()
        {
            if (GameManager.I.DeviceEnviroment == DeviceType.Desktop)
                DestroyImmediate(this);
            camMovement = Camera.main.transform.parent.GetComponent<CameraHandler>();
        }

        public void OnPan(SimpleGestures sender, MultiTouchPointerEventData eventData, Vector2 delta)
        {
            camMovement.TouchPanning(sender, eventData, delta);
        }

        public void OnPinch(SimpleGestures sender, MultiTouchPointerEventData eventData, Vector2 pinchDelta)
        {
            camMovement.PinchToZoom(sender, eventData, pinchDelta);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }
    }
}