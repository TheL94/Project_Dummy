using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Items
{
    public class Potion : ItemGeneric
    {
        public PotionData PotionValues;
        public override void Init(GenericItemData _values)
        {
            PotionValues = _values as PotionData;
            PotionValues.Type = GenericType.Item;
        }
    }
}