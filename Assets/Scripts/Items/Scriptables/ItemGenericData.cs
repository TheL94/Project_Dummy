using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.GDR;

namespace DumbProject.Items
{
    [System.Serializable]
    public class ItemGenericData : GDR_Data_Experience
    {
        public string Name;
        public GameObject ItemPrefab;
        public float TimeToSpend;
        public float PercentageToSpawn;
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