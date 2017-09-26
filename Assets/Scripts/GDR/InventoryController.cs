using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using System.Linq;
using DumbProject.Items;
using DumbProject.Generic;

namespace DumbProject.GDR
{
    public class InventoryController : MonoBehaviour
    {
        List<ItemGeneric> Inventory = new List<ItemGeneric>();
        #region API


        /// <summary>
        /// Raccoglie l'oggetto nella cella selezionata
        /// </summary>
        /// <param name="_gdrController"></param>
        /// <param name="itemToPick"></param>
        public void OnPickUpItem(GDR_Controller _gdrController, ItemGeneric itemToPick)
        {
            OnDropPreviousItem(_gdrController, itemToPick);
            Inventory.Add(itemToPick);
        }
        /// <summary>
        /// Droppa l'oggetto in base al tipo nella cella in cui si trova nel momento in cui viene raccolto il nuovo oggetto.
        /// </summary>
        /// <param name="_gdrController"></param>
        /// <param name="PickedItem"></param>
        public void OnDropPreviousItem(GDR_Controller _gdrController, ItemGeneric PickedItem)
        {
            foreach (ItemGeneric item in Inventory)
            {
                if (item.GetType() == PickedItem.GetType())
                {
                    Cell actualPosition = GameManager.I.MainGridCtrl.GetSpecificGridNode(_gdrController.Data.ai_Controller.transform.position).RelativeCell;
                    item.transform.position = actualPosition.transform.position;
                    item.transform.parent = actualPosition.transform;
                    break;
                }
            }
        }
        #endregion
    }
}
