using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    [System.Serializable]
    public class GenericDroppableData : ScriptableObject
    {
        public string Name;
        public GameObject ItemPrefab;
        public float TimeToSpend;
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