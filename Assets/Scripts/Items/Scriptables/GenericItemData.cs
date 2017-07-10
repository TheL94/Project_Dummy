using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public class GenericItemData : ScriptableObject
    {
        public GenericType Type;
        public Sprite ItemInUI;
        public GameObject ItemPrefab;
        public Material ShowMateriaInRoom;

        public ItemType SpecificItemType;
        public EnemyType SpecificEnemyType;
        public TrapType SpecificTrapType;
    }

    // TODO : da spostare in file separati per evitare Warning

    [CreateAssetMenu(fileName = "Potion", menuName = "Items/Potion Data", order = 0)]
    public class PotionData : GenericItemData
    {
        public float HealtRestore;
    }
    [CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon Data", order = 1)]
    public class WeaponData: GenericItemData
    {
        public float Damage;
    }
    [CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor Data", order = 2)]
    public class ArmorData : GenericItemData
    {
        public float Protection;
    }
    [CreateAssetMenu(fileName = "Enemy", menuName = "Items/Enemy Data", order = 3)]
    public class EnemyData: GenericItemData
    {
        public float Damage;
        public float Life;
    }
    [CreateAssetMenu(fileName = "Trap", menuName = "Items/Trap Data", order = 4)]
    public class TrapData : GenericItemData
    {
        public float Damage;
        public float TimeToLeave;
    }
    [CreateAssetMenu(fileName = "TimeWaster", menuName = "Items/TimeWaster Data", order = 5)]
    public class TimeWasterData : GenericItemData
    {
        public float TimeToLeave;
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