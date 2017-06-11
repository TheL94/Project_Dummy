using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject
{
    [CreateAssetMenu(fileName = "Armory", menuName = "Items/NewArmory", order = 3)]
    public class ArmoryData : ItemBaseData
    {
        public float Protection;
        public float Wear;
    }
}