using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public class DroppableBaseData : ScriptableObject
    {
        public GenericType Type;
        public Sprite ItemInUI;
        public GameObject ItemPrefab;
        public Material ShowMateriaInRoom;

    }

    public enum GenericType
    {
        Ememy = 0,
        Item = 1,
        Trap = 2,
        Gattini = 3,
    }
    
    public enum ItemType
    {
        None = 0,
        Sword = 1,
        Potion = 2,
        Armory = 3,
    }

    public enum EnemyType
    {
        None = 0,
        Drago = 1,
        Goblin = 2,
    }

    public enum TrapType
    {
        None = 0,
        Tagliola = 1,
        Catapulta = 2,
    }
    
}