using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject
{
    public class Sward : MonoBehaviour, IDroppable
    {
        SwardValues values;

        public void GetMyData(BaseData _data)
        {
            values = (_data as SwardData).DataValues;
        }
    }
}