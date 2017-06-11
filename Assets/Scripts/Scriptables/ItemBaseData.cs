using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject
{
    public class ItemBaseData : ScriptableObject
    {
        public GameObject ItemToInstantiate;
        public ItemType Type;
    }
        public enum ItemType
        {
            Potion,
            Sward,
            Armory,
        }
}