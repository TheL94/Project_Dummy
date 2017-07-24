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
            Vector3 objective = new Vector3(camMovement.CurrentPosition.x + eventData.pressPosition.x,
                camMovement.CurrentPosition.y,
                camMovement.CurrentPosition.x + eventData.pressPosition.y);

            camMovement.Translate(objective);
            Debug.Log("drag");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            dragStartPosition = Camera.main.transform.position;
            Debug.Log("click");

        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }

        public void OnScroll(PointerEventData eventData)
        {
            //Vector3 objective;
            //camMovement.Zoom();
        }
    }
}
