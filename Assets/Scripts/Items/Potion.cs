using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public class Potion : MonoBehaviour, IDroppable
    {
        PotionValues values;

        public Transform transF
        {
            get
            {
                return base.transform;
            }
        }

        public void GetMyData(DroppableBaseData _data)
        {
            values = (_data as PotionData).DataValues;
        }
    }
}