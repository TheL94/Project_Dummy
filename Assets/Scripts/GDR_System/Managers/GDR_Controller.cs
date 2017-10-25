using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AI;
using DumbProject.Generic;

namespace DumbProject.GDR_System
{
    public class GDR_Controller : MonoBehaviour
    {
        public Transform RightHand;
        public Transform LeftHand;

        public GDR_Data Data;

        public void Init(GDR_Data _data)
        { 
            Data = _data;
            Data.inventoryCtrl = new InventoryController();
        }

        /// <summary>
        /// Chiamata quando viene raccolto un oggetto da una cella.
        /// </summary>
        public void OnInteract(I_GDR_Interactable _GDR_Interactable, AI_Controller _controller = null)
        {
            if (_GDR_Interactable == null)
            {
                Debug.LogWarning("GDR_Interactable null");
                return;
            }
            else if (_GDR_Interactable.GetType() == typeof(Chest))
            {
                I_GDR_Equippable _GDR_Equippable = (_GDR_Interactable as I_GDR_EquippableHolder).Equippable;

                if (_GDR_Equippable.GetType() == typeof(Potion))
                {
                    Data.GetCure((_GDR_Equippable as Potion).Data.HealtRestore);
                    Destroy(_GDR_Equippable.GameObj);
                    return;
                }
                else if (_GDR_Equippable.GetType() == typeof(Armor))
                {
                    Data.MaxArmor = (_GDR_Equippable as Armor).Data.Protection;
                    Data.inventoryCtrl.OnPickUpItem(this, _GDR_Equippable);
                }
                else if (_GDR_Equippable.GetType() == typeof(Weapon))
                {
                    Data.inventoryCtrl.OnPickUpItem(this, _GDR_Equippable);
                }
            }
            else if (_GDR_Interactable.GetType() == typeof(Trap))
            {
                if (Data.GetDamage((_GDR_Interactable as Trap).Data.Damage))
                    Data.GetExperience(ExperienceType.Trap, _GDR_Interactable.GDR_Data);
                else
                {
                    if (_controller != null)
                        _controller.SetAICurrentState((_controller as Dumby).DefaultDeath);
                }
            }
            else if (_GDR_Interactable.GetType() == typeof(TimeWaster))
            {
                Data.GetExperience(ExperienceType.TimeWaster, _GDR_Interactable.GDR_Data);
            }
            else if (_GDR_Interactable.GetType() == typeof(Enemy))
            {
                if (Data.GetDamage((_GDR_Interactable as Enemy).gdrController.Data.Attack))
                    Data.GetExperience(ExperienceType.Enemy, _GDR_Interactable.GDR_Data);
                else
                {
                    if(_controller != null)
                        _controller.SetAICurrentState((_controller as Dumby).DefaultDeath);
                }

            }
        }
    }
}



