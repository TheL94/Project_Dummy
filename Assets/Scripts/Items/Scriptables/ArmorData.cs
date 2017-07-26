using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor Data", order = 2)]
    public class ArmorData : GenericDroppableData
    {
        public float Protection;
    }
}
