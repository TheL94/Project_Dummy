using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace DumbProject.UI {

    public class InventoryController : MonoBehaviour {

        List<Slot> slots = new List<Slot>();
        public IventoryItem UiItemPrefab;

        List<IventoryItem> itemsInInventory = new List<IventoryItem>();


        // Use this for initialization
        void Start() {
            slots = GetComponentsInChildren<Slot>().ToList();
        }

        #region Item In UI

        /// <summary>
        /// istanzia gli oggetti draggabili nella ui
        /// </summary>
        void InstantiateUIObject()
        {
            // Dovrà creare l'oggetto nella UI relativo all'item raccolto dallo sgherro

            //Slot tempSlot = ChooseFreeSlot();
            //ItemBaseData itemData = ChooseItem();
            //if (tempSlot != null && itemData != null)
            //{
            //    IventoryItem tempItem = Instantiate(UiItemPrefab, new Vector3(tempSlot.transform.position.x, tempSlot.transform.position.y, tempSlot.transform.position.z), Quaternion.identity, tempSlot.transform);
            //    tempSlot.IsFree = false;
            //    tempItem.Init(itemData.ItemPrefab, itemData.ItemInUI, tempSlot);
            //    itemsInInventory.Add(tempItem);
            //}
        }


        /// <summary>
        /// Ritorna uno slot libero in UI
        /// </summary>
        /// <returns></returns>
        Slot ChooseFreeSlot()
        {
            foreach (Slot slot in slots)
            {
                if (slot.IsFree == true)
                    return slot;
            }
            return null;
        }

        #endregion

        #region API

        public void CleanInventory()
        {
            foreach (IventoryItem item in itemsInInventory)
            {
                item.DestroyObj();
            }
        }

        #endregion
    }
}