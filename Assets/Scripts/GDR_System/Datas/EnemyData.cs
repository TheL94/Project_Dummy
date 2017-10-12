using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.GDR_System
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "GDR_Elements/Enemy Data", order = 4)]
    public class EnemyData : GDR_Element_Generic_Data
    {
        public float ActivationRadius;
    }
}
