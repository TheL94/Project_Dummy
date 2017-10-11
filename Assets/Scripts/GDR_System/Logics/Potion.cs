using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using Framework.AI;

namespace DumbProject.GDR_System
{
    public class Potion : MonoBehaviour, IInteractable
    {
        public PotionData Data { get; private set; }

        public bool IsInteractable { get; set; }
        public Transform Transf { get { return transform; } }

        public void Init(GDR_Element_Generic_Data _data)
        {
            Data = _data as PotionData;
        }

        public void Interact(AI_Controller _controller)
        {
            IsInteractable = false;
            //GDR_Controller gdr_controller = _controller.GetComponent<GDR_Controller>();
            //if (gdr_controller != null)
            //{
            //    gdr_controller.OnInteract(this);
            //}
        }
    }
}