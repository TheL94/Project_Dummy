using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [CreateAssetMenu(fileName = "Sward", menuName = "Items/NewSward", order = 2)]
    public class SwardData : BaseData
    {
        public SwardValues DataValues;
    }

    [System.Serializable]
    public struct SwardValues
    {
        public float Damage;
        public float Wear;
    }
}