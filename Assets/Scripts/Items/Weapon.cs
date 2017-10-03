using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using Framework.AI;

namespace DumbProject.Items
{
    public class Weapon : ItemGeneric
    {
        new public WeaponData Data;
        public override void Init(ItemGenericData _values)
        {
            Data = _values as WeaponData;
            Data.Type = GenericType.Item;
        }
    }
}