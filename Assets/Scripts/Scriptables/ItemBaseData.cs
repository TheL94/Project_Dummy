using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject
{
    public class ItemBaseData : ScriptableObject
    {
        public ItemType Type;
        public Sprite ItemInUI;
        public GameObject ItemPrefab;
        // l'interfaccia per tutti gli item
    }
        public enum ItemType
        {
            Potion,
            Sward,
            Armory,
        }
}