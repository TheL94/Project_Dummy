﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public class Potion : MonoBehaviour, IInteractabile
    {
        public PotionValues PotionValues;

        public void Init(PotionValues _values)
        {
            PotionValues = _values;
        }

        public void Interact()
        {
            
        }
    }
}