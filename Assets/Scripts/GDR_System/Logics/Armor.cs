using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using Framework.AI;
using DumbProject.GDR;

namespace DumbProject.GDR_System
{
    public class Armor : MonoBehaviour, I_GDR_Equippable
    {
        public ArmorData Data { get; private set; }

        public void Init(GDR_Element_Generic_Data _data)
        {
            Data = _data as ArmorData;
        }

        #region I_GDR_Equippable
        public GameObject GameObj { get { return gameObject; } }
        #endregion
    }
}