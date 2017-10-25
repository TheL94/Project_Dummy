using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using Framework.AI;

namespace DumbProject.GDR_System
{
    public class Weapon : MonoBehaviour, I_GDR_Equippable, IPreviewable
    {
        public WeaponData Data { get; private set; }

        public void Init(GDR_Element_Generic_Data _data)
        {
            Data = _data as WeaponData;
        }

        #region IPreviewable
        public GameObject PreviewObj { get; set; }
        #endregion

        #region I_GDR_Equippable
        public GameObject GameObj { get { return gameObject; } }
        #endregion
    }
}