using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/GenericData", order = 0)]
    public class GenericData : DroppableBaseData
    {
        public ItemType SpecificType;
        // Sward Parameters
        public SwordValues SwordDataValues;

        // Armory Parameters
        public ArmoryValues ArmoryDataValues;

        // Potion Parameters
        public PotionValues PotionDataValues;
    }

    [SerializeField]
    public struct SwordValues
    {
        public float Damage;
        public float Wear;
    }

    [SerializeField]
    public struct ArmoryValues
    {
        public float Protection;
        public float Wear;
    }

    [SerializeField]
    public struct PotionValues
    {
        public float HealtRestore;
        public float DamageHero;
    }
}