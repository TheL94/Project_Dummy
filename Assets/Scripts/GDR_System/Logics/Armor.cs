using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using Framework.AI;
using DumbProject.GDR;

namespace DumbProject.GDR_System
{
    public class Armor : MonoBehaviour, I_GDR_Equippable, IPreviewable
    {
        public ArmorData Data { get; private set; }

        public void Init(GDR_Element_Generic_Data _data)
        {
            Data = _data as ArmorData;
        }

        #region IPreviewable
        public GameObject PreviewObj { get; set; }
        #endregion

        #region I_GDR_Equippable
        public GameObject GameObj { get { return gameObject; } }

        public void Equip(GDR_Controller _controller) { }
        #endregion
    }
}