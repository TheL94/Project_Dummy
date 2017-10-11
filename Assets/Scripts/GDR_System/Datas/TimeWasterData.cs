using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.GDR_System
{
    [CreateAssetMenu(fileName = "TimeWaster", menuName = "GDR_Elements/TimeWaster Data", order = 5)]
    public class TimeWasterData : GDR_Element_Generic_Data
    {
        public float TimeToSpend;
    }
}