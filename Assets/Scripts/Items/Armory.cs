using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{

    public class Armory : MonoBehaviour, IDroppable
    {
        ArmoryValues values;

        public Transform transF
        {
            get
            {
                return base.transform;
            }
        }

        public void GetMyData(DroppableBaseData _data)
        {
            values = (_data as ArmoryData).DataValues;
        }
    }
}