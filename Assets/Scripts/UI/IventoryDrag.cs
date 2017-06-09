using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DumbProject.UI
{
    public class IventoryDrag : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
    {
        Vector3 _startPosition;
        Transform startParent;
        public static GameObject ItemBeingDragged;

        public void OnPointerDown(PointerEventData eventData)
        {
            ItemBeingDragged = gameObject;
            _startPosition = transform.position;
            startParent = transform.parent;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.position = _startPosition;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            if(transform.parent != startParent)
                transform.position = _startPosition;
            ItemBeingDragged = null;
        }
    }
}