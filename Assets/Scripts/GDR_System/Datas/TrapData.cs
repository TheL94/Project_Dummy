using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.GDR_System
{
    [CreateAssetMenu(fileName = "Trap", menuName = "GDR_Elements/Trap Data", order = 4)]
    public class TrapData : GDR_Element_Generic_Data
    {
        public float Damage;
        public float ActivationRadius;
        public bool ActiveOnce;
    }
}