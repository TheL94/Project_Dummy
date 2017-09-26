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
        List<ItemGeneric> Inventory;

        #region API
        public void Init()
        {
            Inventory = new List<ItemGeneric>();
        }

        /// <summary>
        /// Raccoglie l'oggetto nella cella selezionata
        /// </summary>
        /// <param name="_gdrController"></param>
        /// <param name="itemToPick"></param>
        public void OnPickUpItem(GDR_Controller _gdrController, ItemGeneric itemToPick)
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
        public void OnDropPreviousItem(GDR_Controller _gdrController, ItemGeneric PickedItem)
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i].GetType() == PickedItem.GetType())
                {
                    Cell actualPosition = GameManager.I.MainGridCtrl.GetSpecificGridNode(_gdrController.Data.ai_Controller.transform.position).RelativeCell;
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
                    ItemGeneric item = Inventory[i];
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
        void HeldItem(GDR_Controller _gdrController, ItemGeneric itemToPick)
        {
            itemToPick.transform.parent = _gdrController.transform;
        }
        #endregion
    }
}
