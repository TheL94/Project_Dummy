using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.GDR_System;
using DumbProject.GDR;

namespace DumbProject.Generic
{
    public class GDR_ElementDataManger : MonoBehaviour
    {

        public List<GDR_Element_Generic_Data> GDR_Element_Data = new List<GDR_Element_Generic_Data>();

        [HideInInspector] public List<GDR_Element_Generic_Data> Istances_GDR_Element_Data = new List<GDR_Element_Generic_Data>();

        public void InitData()
        {
            foreach (GDR_Element_Generic_Data element in GDR_Element_Data)
            {
                Istances_GDR_Element_Data.Add(Instantiate(element));
            }
        }      
    }
}
