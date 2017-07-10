﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public abstract class ItemGeneric : MonoBehaviour, IDroppable
    {
        GenericItemData _data;
        public GenericItemData Data
        {
            get { return _data; } 
            set { _data = value; }
        }

        bool _isInteractable;
        public bool IsInteractable { get { return _isInteractable; } set { _isInteractable = value; } }

        public Transform Transf { get { return transform; } }

        public abstract void Init(GenericItemData _data);
        public abstract void Interact();
    }
}