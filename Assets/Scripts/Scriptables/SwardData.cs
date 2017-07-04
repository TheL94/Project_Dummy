using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [CreateAssetMenu(fileName = "Sward", menuName = "Items/NewSward", order = 2)]
    public class SwardData : DroppableBaseData
    {
        public float Damage;
        public float Wear;
    }
}