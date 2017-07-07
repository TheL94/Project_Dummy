using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public class Potion : MonoBehaviour, IInteractable
    {
        public PotionValues PotionValues;

        public void Init(PotionValues _values)
        {
            PotionValues = _values;
        }

        bool _isInteractable = true;
        public bool IsInteractable { get { return _isInteractable; } set { _isInteractable = value; } }

        public void Interact()
        {
            
        }
    }
}