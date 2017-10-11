using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using DumbProject.GDR_System;
using DumbProject.Generic;

namespace DumbProject.GDR
{
    public class InventoryController : MonoBehaviour
    {
        List<I_GDR_Interactable> Inventory;

        #region API
        public void Init()
        {
            Inventory = new List<I_GDR_Interactable>();
        }

        /// <summary>
        /// Raccoglie l'oggetto nella cella selezionata
        /// </summary>
        /// <param name="_gdrController"></param>
        /// <param name="itemToPick"></param>
        public void OnPickUpItem(GDR_Controller _gdrController, I_GDR_Interactable itemToPick)
        {
            OnDropPreviousItem(_gdrController, itemToPick);
            HeldItem(_gdrController, itemToPick);
            Inventory.Add(itemToPick);
        }

        /// <summary>
        /// Droppa l'oggetto in base al tipo nella cella in cui si trova nel momento in cui viene raccolto il nuovo oggetto.
        /// </summary>
        /// <param name="_gdrController"></param>
        /// <param name="PickedItem"></param>
        public void OnDropPreviousItem(GDR_Controller _gdrController, I_GDR_Interactable PickedItem)
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i].GetType() == PickedItem.GetType())
                {
                    Cell actualPosition = GameManager.I.MainGridCtrl.GetSpecificGridNode(_gdrController.transform.position).RelativeCell;
                    Inventory[i].transform.position = actualPosition.transform.position;
                    Inventory[i].transform.parent = actualPosition.transform;
                    Inventory.Remove(Inventory[i]);
                    break;
                }
            }
        }

        public void BreakArmor()
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i].GetType() == typeof(Armor))
                {
                    I_GDR_Interactable item = Inventory[i];
                    Inventory.Remove(item);
                    Destroy(item.gameObject);
                    break;
                }
            }
        }

        /// <summary>
        /// Impugna l'oggetto che raccoglie 
        /// </summary>
        /// <param name="_gdrController"></param>
        /// <param name="itemToPick"></param>
        void HeldItem(GDR_Controller _gdrController, I_GDR_Interactable itemToPick)
        {
            itemToPick.transform.parent = _gdrController.transform;
        }
        #endregion
    }
}
