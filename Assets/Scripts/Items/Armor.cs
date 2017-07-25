using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Items
{
    public class Armor : ItemGeneric
    {
        public ArmorData ArmoryValues;
        public override void Init(GenericItemData _values)
        {
            ArmoryValues = _values as ArmorData;
        }
    }
}