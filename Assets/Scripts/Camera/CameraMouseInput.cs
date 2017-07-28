using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DumbProject.CameraController;

namespace DumbProject.Generic
{
    public class CameraMouseInput : UIBehaviour, IDragHandler, IScrollHandler, IPointerDownHandler
    {
        CameraHandler camHandler;

        new private void Start()
        {
            if (GameManager.I.DeviceEnviroment == DeviceType.Handheld)
                DestroyImmediate(this);
            camHandler = Camera.main.transform.parent.GetComponent<CameraHandler>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            camHandler.OnDragEvent(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            camHandler.OnClickEvent(eventData);
        }

        public void OnScroll(PointerEventData eventData)
        {
            camHandler.OnScrollEvent(eventData);
        }
    }
}
