using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public class Potion : MonoBehaviour, IDroppable
    {
        public PotionData SpecificValues { get { return Data as PotionData; } }
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