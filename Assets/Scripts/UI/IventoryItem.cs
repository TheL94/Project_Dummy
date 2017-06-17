using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DumbProject.UI
{
    public class IventoryItem : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
    {
        Vector3 _startPosition;
        [HideInInspector]
        public GameObject ItemToInstantiate;
        Slot mySlot;

        public void Init(GameObject _itemToInstantiate, Sprite _uiItemSprite, Slot _ownSlot)
        {
            ItemToInstantiate = _itemToInstantiate;
            GetComponent<Image>().sprite = _uiItemSprite;
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