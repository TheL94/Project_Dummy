using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [CreateAssetMenu(fileName = "Armory", menuName = "Items/NewArmory", order = 3)]
    public class ArmoryData : DroppableBaseData
    {
        public ItemType SpecificType;
        public float Protection;
        public float Wear;
    }
}