using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Items;
using DumbProject.GDR;


namespace DumbProject.Generic
{
    public class FillerDataManger : MonoBehaviour {

        public List<ItemGenericData> itemsData = new List<ItemGenericData>();
        public List<GDR_Data_Experience> gdr_data = new List<GDR_Data_Experience>();

        [HideInInspector] public List<ItemGenericData> Istances_itemsData = new List<ItemGenericData>();
        [HideInInspector] public List<TrapData> Istances_gdr_data = new List<TrapData>();


        public void InitData() {
            foreach (ItemGenericData item in itemsData)
            {
                Istances_itemsData.Add(Instantiate(item));
            }
            foreach (TrapData item in gdr_data)
            {
                Istances_gdr_data.Add(Instantiate(item));
            }
        }
        
    }
}
