using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using Framework.AI;

namespace DumbProject.GDR_System
{
    public class Chest : MonoBehaviour, IInteractable, I_GDR_Interactable, I_GDR_EquippableHolder
    {
        public ChestData Data { get; private set; }

        InteractionAnimator interactionAnimator;

        public void Init(GDR_Element_Generic_Data _data)
        {
            Data = _data as ChestData;
            interactionAnimator = GetComponent<InteractionAnimator>();
            IsInteractable = true;
        }

        #region I_GDR_EquippableHolder
        public I_GDR_Equippable Equippable { get; set; }

        public void EnableEquippable(bool _status)
        {
            foreach (MeshRenderer mesh in Equippable.GameObj.GetComponentsInChildren<MeshRenderer>())
            {
                mesh.enabled = _status;
            }
        }
        #endregion

        #region IInteractable
        public bool IsInteractable { get; set; }
        public Transform Transf { get { return transform; } }

        public void Interact(AI_Controller _controller)
        {
            IsInteractable = false;
            interactionAnimator.OpenAsChest();
            GDR_Interact(_controller.GetComponent<GDR_Controller>());
        }
        #endregion

        #region I_GDR_Interactable
        public GDR_Element_Generic_Data GDR_Data { get { return Data as GDR_Element_Generic_Data; } }

        public void GDR_Interact(GDR_Controller _GDR_Controller)
        {
            EnableEquippable(true);
            if (_GDR_Controller != null)
                _GDR_Controller.OnInteract(this);
        }
        #endregion
    }
}

