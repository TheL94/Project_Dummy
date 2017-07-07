using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public class Sword : MonoBehaviour, IInteractable
    {

        public SwordValues SwordValues;

        public void Init(SwordValues _values)
        {
            SwordValues = _values;
        }

        bool _isInteractable = true;
        public bool IsInteractable { get { return _isInteractable; } set { _isInteractable = value; } }

        public void Interact()
        {
            
        }
    }
}