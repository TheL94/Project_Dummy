using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [CreateAssetMenu(fileName = "Potion", menuName = "Items/Potion Data", order = 0)]
    public class PotionData : GenericDroppableData
    {
        public float HealtRestore;
    }
}