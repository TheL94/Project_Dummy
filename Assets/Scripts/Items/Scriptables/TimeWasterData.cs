using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [CreateAssetMenu(fileName = "TimeWaster", menuName = "Items/TimeWaster Data", order = 5)]
    public class TimeWasterData : GenericDroppableData
    {
        public float TimeToLeave;
    }
}
