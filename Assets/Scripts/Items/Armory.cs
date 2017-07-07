using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{

    public class Armory : MonoBehaviour, IInteractable
    {
        public ArmoryValues ArmoryValues;

        public void Init(ArmoryValues _values)
        {
            ArmoryValues = _values;
        }

        bool _isInteractable = true;
        public bool IsInteractable { get { return _isInteractable; } set { _isInteractable = value; } }

        public void Interact()
        {

        }
    }
}