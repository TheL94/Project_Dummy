using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [System.Serializable]
    public class GenericDroppableData : ScriptableObject
    {
        public string Name;
        public GameObject ItemPrefab;
        public float TimeToSpend;
        public float PercentageToSpawn;

        public GenericType Type;
        public ItemType SpecificItemType;
        public EnemyType SpecificEnemyType;
        public TrapType SpecificTrapType;
    }

    public enum GenericType
    {
        Ememy = 0,
        Item = 1,
        Trap = 2,
        TimeWaster = 3
    }
    
    public enum ItemType
    {
        None = 0,
        Weapon = 1,
        Potion = 2,
        Armor = 3
    }

    public enum EnemyType
    {
        None = 0,
        Dragon = 1,
        Goblin = 2
    }

    public enum TrapType
    {
        None = 0,
        Type1 = 1,
        Type2 = 2
    }
    
}