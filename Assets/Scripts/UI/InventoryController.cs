using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace DumbProject.UI {

    public class InventoryController : MonoBehaviour {

        List<Slot> slots = new List<Slot>();
        public List<ItemBaseData> ItemsData = new List<ItemBaseData>();
        public IventoryItem UiItemPrefab;

        // Use this for initialization
        void Start() {
            slots = GetComponentsInChildren<Slot>().ToList();
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown(KeyCode.X))
                InstantiateUIObject();
        }
        
        /// <summary>
        /// istanzia gli oggetti draggabili nella ui
        /// </summary>
        void InstantiateUIObject()
        {
            Slot tempSlot = ChooseFreeSlot();
            ItemBaseData itemData = ChooseItem();
            if (tempSlot != null && itemData != null)
            {
                IventoryItem tempItem = Instantiate(UiItemPrefab, new Vector3(tempSlot.transform.position.x, tempSlot.transform.position.y, tempSlot.transform.position.z), Quaternion.identity, tempSlot.transform);
                tempSlot.IsFree = false;
                tempItem.Init(itemData.ItemPrefab, itemData.ItemInUI, tempSlot);
            }
        }

        /// <summary>
        /// Ritorna l'item da istanziare nella ui
        /// </summary>
        /// <returns></returns>
        ItemBaseData ChooseItem()
        {
            int randNum = Random.Range(0, ItemsData.Count);
            return ItemsData[randNum];
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
    }
}