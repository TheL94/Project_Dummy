using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DumbProject.Items
{
    [CreateAssetMenu(fileName = "Trap", menuName = "Items/Trap Data", order = 4)]
    public class TrapData : GDR.GDR_Data_Experience
    {
        public float Damage;
        public float ActivationRadius;
        public float TimeToLeave;
        public string Name;
        public GameObject ItemPrefab;
        public float PercentageToSpawn;
    }
}