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
    
}