using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.grey;
            if (!IsInteractable)
                Gizmos.DrawCube(transform.position + new Vector3(0, 2, 0), Vector3.one);
        }
    }
}