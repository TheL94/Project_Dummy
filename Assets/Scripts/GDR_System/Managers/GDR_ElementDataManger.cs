using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.GDR_System;

namespace DumbProject.Generic
{
    public class GDR_ElementDataManger : MonoBehaviour
    {

        public List<GDR_Element_Generic_Data> GDR_Element_Data = new List<GDR_Element_Generic_Data>();
        [HideInInspector] public List<GDR_Element_Generic_Data> Istances_GDR_Element_Data = new List<GDR_Element_Generic_Data>();

        public List<GDR_Data> GDR_Data = new List<GDR_Data>();
        [HideInInspector] public List<GDR_Data> Istances_GDR_Data = new List<GDR_Data>();

        public void InitData()
        {
            foreach (GDR_Element_Generic_Data element in GDR_Element_Data)
            {
                Istances_GDR_Element_Data.Add(Instantiate(element));
            }

            foreach (GDR_Data data in GDR_Data)
            {
                Istances_GDR_Data.Add(Instantiate(data));
            }
        }  
        
        public GDR_Data GetGDR_DataByID(string _id)
        {
            foreach (GDR_Data data in Istances_GDR_Data)
            {
                if (data.ID == _id)
                    return data;
            }
            return null;
        }
    }
}
