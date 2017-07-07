using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public class Sword : MonoBehaviour, IInteractabile
    {

        public SwordValues SwordValues;

        public void Init(SwordValues _values)
        {
            SwordValues = _values;
        }

        public void Interact()
        {
            
        }
    }
}