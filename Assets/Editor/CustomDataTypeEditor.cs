using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DumbProject.Items;

namespace DumbProject.Editors
{
    [CustomEditor(typeof(SwardData))]
    public class CustomDataTypeEditor : Editor
    {
        SwardData data;

        private void OnEnable()
        {
            data = (SwardData)target;
        }
        private List<GenericType> _categories;
        private List<string> _categoryLabels;
        int _categorySelected = 0;

        private void InitCategories() { Debug.Log("InitCategories called...");
            _categories = GetListFromEnum<GenericType>();
            _categoryLabels = new List<string>();
            foreach (GenericType category in _categories) { _categoryLabels.Add(category.ToString()); } }

        public static List<T> GetListFromEnum<T>() { List<T> enumList = new List<T>(); System.Array enums = System.Enum.GetValues(typeof(T)); foreach (T e in enums) { enumList.Add(e); } return enumList; }


        public override void OnInspectorGUI()
        {
            DrawTabs();

            GUILayout.BeginHorizontal();
            //data.Type = (DumbProject.Items.GenericType)EditorGUILayout.EnumPopup("Generic Type", data.Type);
            InitCategories();

            if (_categorySelected == 0)
            {
                GUILayout.Label("Danni" + data.ItemPrefab);
                data.Damage = EditorGUILayout.Slider(data.Damage, 2, 8);
            }

            GUILayout.EndHorizontal();
            base.OnInspectorGUI();
        }

        private void DrawTabs() {
            int index = (int)_categorySelected;
            index = GUILayout.Toolbar(index, _categoryLabels.ToArray());
            _categorySelected = (int)_categories[index];
        }

    }
}