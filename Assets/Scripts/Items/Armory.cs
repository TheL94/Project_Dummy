using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject
{

    public class Armory : MonoBehaviour, IDroppable
    {
        ArmoryValues values;

        public Transform position
        {
            get
            {
                return transform;
            }
        }

        public void GetMyData(BaseData _data)
        {
            values = (_data as ArmoryData).DataValues;
        }
    }
}