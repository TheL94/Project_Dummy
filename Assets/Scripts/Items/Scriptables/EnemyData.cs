using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Items/Enemy Data", order = 3)]
    public class EnemyData : GenericDroppableData
    {
        public float Damage;
        public float Life;
    }
}
