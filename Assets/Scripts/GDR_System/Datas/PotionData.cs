using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.GDR_System
{
    [CreateAssetMenu(fileName = "Potion", menuName = "Items/Potion Data", order = 0)]
    public class PotionData : GDR_Element_Generic_Data
    {
        public float HealtRestore;
    }
}