using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{

    public class Armory : MonoBehaviour, IDroppable
    {
        public ArmoryData SpecificValues { get { return Data as ArmoryData; } }

        public Transform transF
        {
            get
            {
                return base.transform;
            }
        }

        DroppableBaseData _data;

        public DroppableBaseData Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public void InitIDroppable(DroppableBaseData _data)
        {
            Data = _data;
        }
    }
}