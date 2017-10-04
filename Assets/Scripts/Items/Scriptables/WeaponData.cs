using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon Data", order = 1)]
    public class WeaponData : ItemGenericData
    {
        public float Damage;
    }
}
