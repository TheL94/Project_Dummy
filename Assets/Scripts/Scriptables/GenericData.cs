using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/GenericData", order = 0)]
    public class GenericData : DroppableBaseData
    {
        public ItemType SpecificItemType;
        public EnemyType SpecificEnemyType;
        public TrapType SpecificTrapType;
        // Sward Parameters
        public SwordValues SwordDataValues;

        // Armory Parameters
        public ArmoryValues ArmoryDataValues;

        // Potion Parameters
        public PotionValues PotionDataValues;

        // Dragon Parameters
        public DragonValues DragonDataValues;

        // Goblin Parameters
        public GoblinValues GoblinDataValues;

        // Tagliola Parameters
        public TagliolaValues TagliolaValues;

        // Catapulta Parameters
        public CatapultaValues CatapultaValues;

    }


    [System.Serializable]
    public struct SwordValues
    {
        public float Damage;
        public float Wear;
    }

    [System.Serializable]
    public struct ArmoryValues
    {
        public float Protection;
        public float Wear;
    }

    [System.Serializable]
    public struct PotionValues
    {
        public float HealtRestore;
        public float DamageHero;
    }

    [System.Serializable]
    public struct DragonValues
    {
        public float Damage;
        public float Life;
    }

    [System.Serializable]
    public struct GoblinValues
    {
        public float Damage;
        public float Life;
    }

    [System.Serializable]
    public struct TagliolaValues
    {
        public float Damage;
        public float TimeToLeave;
    }

    [System.Serializable]
    public struct CatapultaValues
    {
        public float Damage;
        public float TimeToLeave;
    }
}