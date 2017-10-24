using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using Framework.AI;

namespace DumbProject.GDR_System
{
    public class Chest : MonoBehaviour, IInteractable, I_GDR_EquippableHolder
    {
        #region IInteractable
        public bool IsInteractable { get; set; }
        public Transform Transf { get { return transform; } }

        public void Interact(AI_Controller _controller)
        {
            IsInteractable = false;           
        }
        #endregion
    }
}

