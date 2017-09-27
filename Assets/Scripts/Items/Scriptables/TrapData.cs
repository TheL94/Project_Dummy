using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [CreateAssetMenu(fileName = "Trap", menuName = "Items/Trap Data", order = 4)]
    public class TrapData : GenericDroppableData
    {
        public float Damage;
        public float ActivationRadius;
    }
}