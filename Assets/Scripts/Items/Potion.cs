using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject
{
    public class Potion : MonoBehaviour, IDroppable
    {
        PotionValues values;

        public Transform transform
        {
            get
            {
                return base.transform;
            }
        }

        public void GetMyData(BaseData _data)
        {
            values = (_data as PotionData).DataValues;
        }
    }
}