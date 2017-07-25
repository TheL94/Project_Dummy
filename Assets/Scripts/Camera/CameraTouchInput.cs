using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DumbProject.CameraController;
using Multitouch.EventSystems.EventData;
using Multitouch.EventSystems.Gestures;
using System;

namespace DumbProject.Generic
{
    public class CameraTouchInput : UIBehaviour, IPanHandler, IPinchHandler, IPointerDownHandler
    {
        CameraHandler camMovement;

        new private void Start()
        {
            if (GameManager.I.DeviceEnviroment == DeviceType.Desktop)
                DestroyImmediate(this);
            camMovement = Camera.main.GetComponent<CameraHandler>();
        }

        public void OnPan(SimpleGestures sender, MultiTouchPointerEventData eventData, Vector2 delta)
        {
            
        }

        public void OnPinch(SimpleGestures sender, MultiTouchPointerEventData eventData, Vector2 pinchDelta)
        {
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }
    }
}