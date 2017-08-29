using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class FlexibleUIComponent : MonoBehaviour
    {
        public UIPositionData UIObjectData;
        public bool FillAnchors;

        void UpdateRectTransform()
        {
            if (FillAnchors)
                GameManager.I.UIMng.SetRectTransformParametersByValues(transform as RectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            else if (UIObjectData != null)
                GameManager.I.UIMng.SetRectTransformParametersByData(transform as RectTransform, UIObjectData);
            else
                Debug.LogWarning("Missing UI Data and Fill Anchors disabled on " + name + " object !");
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

