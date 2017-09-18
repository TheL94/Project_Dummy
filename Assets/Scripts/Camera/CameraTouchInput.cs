using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DumbProject.CameraController;

namespace DumbProject.Generic
{
    public class CameraTouchInput : CameraInput, IDragHandler, IPointerDownHandler
    {
        public void OnDrag(PointerEventData eventData)
        {
            camHandler.OnDragEvent(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            camHandler.OnClickEvent(eventData);
        }
    }
}