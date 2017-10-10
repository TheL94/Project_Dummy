using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FlexibleUI
{
    [System.Serializable]
    [CanEditMultipleObjects]
    [CustomEditor(typeof(FlexibleUIComponent))]
    public class FlexibleUIComponentEditor : Editor
    {
        FlexibleUIComponent flexibleUIComponent;

        private void OnEnable()
        {
            flexibleUIComponent = (FlexibleUIComponent)target;
        }

        public override void OnInspectorGUI()
        {
            if (flexibleUIComponent.VerticalUIObjectData == null && flexibleUIComponent.HorizontalUIObjectData == null)
            {
                flexibleUIComponent.FillParentAnchors = EditorGUILayout.Toggle("Fill Parent Anchors", flexibleUIComponent.FillParentAnchors);
            }

            EditorGUILayout.Space();

            if (!flexibleUIComponent.FillParentAnchors)
            {
                flexibleUIComponent.VerticalUIObjectData = EditorGUILayout.ObjectField("Vertical UI Object Data", flexibleUIComponent.VerticalUIObjectData, typeof(FlexibleUIData), false) as FlexibleUIData;
                flexibleUIComponent.HorizontalUIObjectData = EditorGUILayout.ObjectField("Horizontal UI Object Data", flexibleUIComponent.HorizontalUIObjectData, typeof(FlexibleUIData), false) as FlexibleUIData;
            }
        }
    }
}
