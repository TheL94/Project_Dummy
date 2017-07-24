using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DumbProject.Generic
{
    public class CameraController : MonoBehaviour, IDragHandler, IScrollHandler, IPointerDownHandler, IPointerUpHandler
    {
        CameraMovement camMovement;

        Vector3 dragStartPosition;
        Vector3 dragEndPosition;

        private void Start()
        {
            camMovement = Camera.main.GetComponent<CameraMovement>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            //camMovement.Drag();
            //Debug.Log("drag");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            dragStartPosition = eventData.pressPosition;
        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }

        public void OnScroll(PointerEventData eventData)
        {
            //camMovement.Zoom();
        }
    }
}
