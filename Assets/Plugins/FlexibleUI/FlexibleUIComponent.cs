using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlexibleUI
{
    [AddComponentMenu("FlexibleUI/UIComponent")]
    public class FlexibleUIComponent : MonoBehaviour
    {
        public FlexibleUIData VerticalUIObjectData;
        public FlexibleUIData HorizontalUIObjectData;
        public bool FillParentAnchors;

        void UpdateRectTransform()
        {
            if (FillParentAnchors)
                FlexibleUIManager.SetRectTransformParametersByValues(transform as RectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            else if (VerticalUIObjectData != null)
                FlexibleUIManager.SetRectTransformParametersByData(transform as RectTransform, VerticalUIObjectData, HorizontalUIObjectData);
            else
                Debug.LogWarning("Missing UI Data and Fill Parent Anchors disabled on " + name + " object !");
        }

        private void OnEnable()
        {
            FlexibleUIManager.OnScreenOrientationChange += UpdateRectTransform;
            UpdateRectTransform();
        }

        private void OnDisable()
        {
            FlexibleUIManager.OnScreenOrientationChange -= UpdateRectTransform;
        }
    }
}

