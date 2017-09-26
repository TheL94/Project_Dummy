using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.GDR;
using Framework.AI;

namespace DumbProject.Items
{
    public abstract class ItemGeneric : MonoBehaviour, IDroppable
    {
        GenericDroppableData _data;
        public GenericDroppableData Data
        {
            get { return _data; } 
            set { _data = value; }
        }

        bool _isInteractable = true;
        public bool IsInteractable { get { return _isInteractable; } set { _isInteractable = value; } }

        public Transform Transf { get { return transform; } }

        public abstract void Init(GenericDroppableData _data);
        public virtual void Interact(AI_Controller _controller)
        {
            IsInteractable = false;
            GDR_Controller gdr_controller = _controller.GetComponent<GDR_Controller>();
            if (gdr_controller != null)
            {
                gdr_controller.OnInteract(this);
            }
        }
    }
}