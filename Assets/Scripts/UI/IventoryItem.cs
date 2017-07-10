using System;
using System.Collections;
using DumbProject.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DumbProject.UI
{
    public class IventoryItem : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
    {
        Vector3 _startPosition;
        [HideInInspector]
        public GenericItemData DroppableData;
        Slot mySlot;

        public void Init(GenericItemData _droppableData, Slot _ownSlot)
        {
            DroppableData = _droppableData;
            GetComponent<Image>().sprite = _droppableData.ItemInUI;
            mySlot = _ownSlot;
        }

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

        /// <summary>
        /// Riporta l'oggetto nella posizione iniziale
        /// </summary>
        public void PlaceInStartPosition()
        {
            transform.position = _startPosition;
        }

        /// <summary>
        /// Distrugge l'oggetto liberando lo slot a cui era assegnato
        /// </summary>
        public void DestroyObj()
        {
            Destroy(gameObject);
            mySlot.IsFree = true;
        }
    }
       
}