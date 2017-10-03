using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlexibleUI
{
    [CreateAssetMenu(fileName = "FlexibleUIData", menuName = "UI/NewFlexibleUIData", order = 0)]
    public class FlexibleUIData : ScriptableObject
    {
        public RectTranformData RectTranformValues;
    }
}
