using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using Framework.AI;
using DumbProject.GDR;

namespace DumbProject.GDR_System
{
    public class Armor : MonoBehaviour, IInteractable, I_GDR_Interactable
    {
        public ArmorData Data { get; private set; }

        public bool IsInteractable { get; set; }
        public Transform Transf { get { return transform; } }

        public void Init(GDR_Element_Generic_Data _data)
        {
            Data = _data as ArmorData;
        }

        public void Interact(AI_Controller _controller)
        {
            IsInteractable = false;
            GDR_Interact(_controller.GetComponent<GDR_Controller>());
        }

        public void GDR_Interact(GDR_Controller _GDR_Controller)
        {
            if (_GDR_Controller != null)
                _GDR_Controller.OnInteract(this);
        }
    }
}