using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [CreateAssetMenu(fileName = "Potion", menuName = "Items/NewPotion", order = 1)]
    public class PotionData : BaseData
    {
        public PotionValues DataValues;
    }

    [System.Serializable]
    public struct PotionValues
    {
        public float HealtRestore;
        public float DamageHero;
    }
}