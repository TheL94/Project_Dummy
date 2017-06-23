using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "ItemData/NewPotion", order = 2)]
    public class PotionData : ItemData
    {
        public float Heal;
    }
}
