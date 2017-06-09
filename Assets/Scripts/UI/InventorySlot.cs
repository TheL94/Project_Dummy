using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DumbProject.UI
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {

        public GameObject Child
        {
            get
            {
                if (transform.childCount > 1)
                    return transform.GetChild(0).gameObject;
                else
                    return null;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (!Child)
            {
                IventoryDrag.ItemBeingDragged.transform.SetParent(transform);
            }
        }
    }
}