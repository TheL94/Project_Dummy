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
        Item,
        Trap,
        Gattini,
    }
    
    public enum ItemType
    {
        Sword = 0,
        Potion = 1,
        Armory = 2,
    }

    public enum EnemyType
    {
        Drago = 0,
        Goblin = 1,
    }

    public enum TrapType
    {
        Tagliola = 0,
        Catapulta = 1,
    }
    
}