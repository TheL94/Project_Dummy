using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public class BaseData : ScriptableObject
    {
        public ItemType Type;
        public Sprite ItemInUI;
        public GameObject ItemPrefab;
        public Material ShowMateriaInRoom;
    }
        public enum ItemType
        {
            Item,
            Ememy,
            Trap,
            Gattini,
        }
}