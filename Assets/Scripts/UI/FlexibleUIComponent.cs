using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class FlexibleUIComponent : MonoBehaviour
    {
        public UIPositionData UIObjectData;

        void UpdateRectTransform()
        {
            GameManager.I.UIMng.SetRectTransformParametersByData(transform as RectTransform, UIObjectData);
        }

        private void OnEnable()
        {
            GameManager.I.UIMng.OnScreenOrientationChange += UpdateRectTransform;
        }

        private void OnDisable()
        {
            GameManager.I.UIMng.OnScreenOrientationChange -= UpdateRectTransform;
        }

    }
}

