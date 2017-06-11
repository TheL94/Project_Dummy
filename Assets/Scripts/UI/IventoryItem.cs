using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DumbProject.UI
{
    public class IventoryItem : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
    {
        Vector3 _startPosition;
        public GameObject ItemToInstantiate;

        public void OnPointerDown(PointerEventData eventData)
        {
            _startPosition = transform.position;
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
        }

    }
       
}