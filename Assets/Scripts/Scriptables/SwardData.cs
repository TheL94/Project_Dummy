using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "ItemData/NewSward", order = 1)]
    public class SwardData : ItemData
    {
        public float Damage;
    }
}
