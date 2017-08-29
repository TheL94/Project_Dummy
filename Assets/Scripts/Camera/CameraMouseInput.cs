using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DumbProject.Generic
{
    public class CameraMouseInput : CameraInput, IDragHandler, IScrollHandler, IPointerDownHandler
    {
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
