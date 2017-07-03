using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [CreateAssetMenu(fileName = "Armory", menuName = "Items/NewArmory", order = 3)]
    public class ArmoryData : BaseData
    {
        public ArmoryValues DataValues;
    }

    [System.Serializable]
    public struct ArmoryValues
    {
        public float Protection;
        public float Wear;
    }
}