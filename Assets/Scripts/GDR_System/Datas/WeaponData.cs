using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.GDR_System
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "GDR_Elements/Weapon Data", order = 1)]
    public class WeaponData : GDR_Element_Generic_Data
    {
        public float Damage;
    }
}
