using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public class Sward : MonoBehaviour, IDroppable
    {
        SwardValues values;

        public Transform transF
        {
            get
            {
                return base.transform;
            }
        }

        public void GetMyData(DroppableBaseData _data)
        {
            values = (_data as SwardData).DataValues;
        }
    }
}