using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.GDR_System
{
    [CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor Data", order = 2)]
    public class ArmorData : GDR_Element_Generic_Data
    {
        public float Protection;
    }
}
