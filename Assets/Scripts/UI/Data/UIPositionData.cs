using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.UI
{
    [CreateAssetMenu(fileName = "UIPositionData", menuName = "UI/NewUIPositionData", order = 0)]
    public class UIPositionData : ScriptableObject
    {
        public RectTranformValues verticalRectValues;
        public RectTranformValues orizontalRectValues;
    }
}

