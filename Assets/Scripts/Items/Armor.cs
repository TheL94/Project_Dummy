using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Items
{
    public class Armor : ItemGeneric
    {
        new public ArmorData Data;
        public override void Init(ItemGenericData _values)
        {
            Data = _values as ArmorData;
            Data.Type = GenericType.Item;
        }
    }
}