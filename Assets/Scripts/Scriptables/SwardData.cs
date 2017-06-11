using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject
{
    [CreateAssetMenu(fileName = "Sward", menuName = "Items/NewSward", order = 2)]
    public class SwardData : ItemBaseData
    {
        public float Damage;
        public float Wear;
    }
}