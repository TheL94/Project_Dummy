using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Items
{
    public class Weapon : ItemGeneric
    {
        public WeaponData SwordValues;
        public override void Init(GenericDroppableData _values)
        {
            SwordValues = _values as WeaponData;
            SwordValues.Type = GenericType.Item;
        }
    }
}