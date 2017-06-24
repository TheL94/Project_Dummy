using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public class ItemData : BaseData
    {
        public ItemType SpecificType;
    }

    public enum ItemType
    {
        Sward,
        Potion
    }
}