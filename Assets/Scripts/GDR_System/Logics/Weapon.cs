using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using Framework.AI;
using DumbProject.GDR;

namespace DumbProject.GDR_System
{
    public class Weapon : MonoBehaviour, I_GDR_Equippable
    {
        public WeaponData Data { get; private set; }

        public void Init(GDR_Element_Generic_Data _data)
        {
            Data = _data as WeaponData;
        }

        #region I_GDR_Equippable
        public GameObject GameObj { get { return gameObject; } }
        #endregion
    }
}