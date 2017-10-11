using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.GDR_System
{
    [System.Serializable]
    public abstract class GDR_Element_Generic_Data : ScriptableObject
    {
        public GameObject ElementPrefab;
        public float PercentageToSpawn;
        public float ExperienceToGive;
        public GenericType Type;
    }

    public enum GenericType
    {
        Ememy = 0,
        Item = 1,
        Trap = 2,
        TimeWaster = 3
    }
}