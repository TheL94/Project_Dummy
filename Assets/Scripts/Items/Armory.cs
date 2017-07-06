using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{

    public class Armory : MonoBehaviour, IInteractabile
    {
        public ArmoryValues ArmoryValues;

        public void Init(ArmoryValues _values)
        {
            ArmoryValues = _values;
        }

        public void Interact()
        {

        }
    }
}