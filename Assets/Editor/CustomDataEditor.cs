using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DumbProject.Items;

namespace DumbProject.Editors
{
    [System.Serializable]
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GenericDroppableData), true)]
    public class CustomDataTypeEditor : Editor
    {
        GenericDroppableData Data;
       
        private void OnEnable()
        {
            Data = (GenericDroppableData)target;
        }

        List<GenericType> _genericCategories;
        List<ItemType> _itemCategory;
        List<EnemyType> _enemyCategory;
        List<TrapType> _trapCategory;

        private List<string> _genericCategoryLabels;
        private List<string> _itemCategoryLabels;
        private List<string> _enemyCategoryLabels;
        private List<string> _trapCategoryLabels;

        int _genericCategorySelected { get { return (int)Data.Type; } set { Data.Type = (GenericType)value; } }
        int _itemCategorySelected { get { return (int)Data.SpecificItemType; } set { Data.SpecificItemType = (ItemType)value; } }
        int _enemyCategorySelected { get { return (int)Data.SpecificEnemyType; } set { Data.SpecificEnemyType = (EnemyType)value; } }
        int _trapCategorySelected { get { return (int)Data.SpecificTrapType; } set { Data.SpecificTrapType = (TrapType)value; } }


        private void InitCategories() { 
            _genericCategories = GetListFromEnum<GenericType>();
            _itemCategory = GetListFromEnum<ItemType>();
            _enemyCategory = GetListFromEnum<EnemyType>();
            _trapCategory = GetListFromEnum<TrapType>();

            _genericCategoryLabels = new List<string>();
            _itemCategoryLabels = new List<string>();
            _enemyCategoryLabels = new List<string>();
            _trapCategoryLabels = new List<string>();
            SaveCategories();
        }

        void SaveCategories()
        {
            foreach (GenericType category in _genericCategories)
            {
                _genericCategoryLabels.Add(category.ToString());
            }
            foreach (ItemType category in _itemCategory)
            {
                _itemCategoryLabels.Add(category.ToString());
            }
            foreach (EnemyType category in _enemyCategory)
            {
                _enemyCategoryLabels.Add(category.ToString());
            }
            foreach (TrapType category in _trapCategory)
            {
                _trapCategoryLabels.Add(category.ToString());
            }
        }

        public static List<T> GetListFromEnum<T>() {
            List<T> enumList = new List<T>();
            System.Array enums = System.Enum.GetValues(typeof(T));
            foreach (T e in enums)
            {
                enumList.Add(e);
            }
            return enumList;
        }


        public override void OnInspectorGUI()
        {
            InitCategories();
            //DrawGenericTabs();

            if(Data.GetType() == typeof(EnemyData))
            {
                DrawEnemyTabs();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Damage");
                GUILayout.Space(100);
                (Data as EnemyData).Damage = EditorGUILayout.FloatField((Data as EnemyData).Damage);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Life");
                GUILayout.Space(100);
                (Data as EnemyData).Life = EditorGUILayout.FloatField((Data as EnemyData).Life);
                GUILayout.EndHorizontal();
            }

            if (Data.GetType() == typeof(WeaponData))
            {
                //DrawItemTabs();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Damage");
                GUILayout.Space(100);
                (Data as WeaponData).Damage = EditorGUILayout.FloatField((Data as WeaponData).Damage);
                GUILayout.EndHorizontal();
            }

            if (Data.GetType() == typeof(PotionData))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Healt Restore");
                GUILayout.Space(100);
                (Data as PotionData).HealtRestore = EditorGUILayout.FloatField((Data as PotionData).HealtRestore);
                GUILayout.EndHorizontal();
            }

            if (Data.GetType() == typeof(ArmorData))
            {
                //DrawItemTabs();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Protection");
                GUILayout.Space(100);
                (Data as ArmorData).Protection = EditorGUILayout.FloatField((Data as ArmorData).Protection);
                GUILayout.EndHorizontal();
            }            

            if(Data.GetType() == typeof(TrapData))
            {
                DrawTrapsTabs();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Damage");
                GUILayout.Space(100);
                (Data as TrapData).Damage = EditorGUILayout.FloatField((Data as TrapData).Damage);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Time To Leave");
                GUILayout.Space(100);
                (Data as TrapData).TimeToLeave = EditorGUILayout.FloatField((Data as TrapData).TimeToLeave);
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(15);
            //The sprite to show in the Inventory
            GUILayout.BeginHorizontal();
            Data.ItemInUI = (Sprite)EditorGUILayout.ObjectField("Sprite for Invetory", Data.ItemInUI, typeof(Sprite), false);
            GUILayout.EndHorizontal();

            // The prefab to instantiate
            GUILayout.BeginHorizontal();
            Data.ItemPrefab = (GameObject)EditorGUILayout.ObjectField("ItemPrefab", Data.ItemPrefab, typeof(GameObject), false);
            GUILayout.EndHorizontal();

            //The Material to apply at the floor in the room
            GUILayout.BeginHorizontal();
            Data.ShowMateriaInRoom = (Material)EditorGUILayout.ObjectField("Material For Room", Data.ShowMateriaInRoom, typeof(Material), false);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Time to spend");
            GUILayout.Space(80);
            (Data as GenericDroppableData).TimeToSpend = EditorGUILayout.FloatField((Data as GenericDroppableData).TimeToSpend);
            GUILayout.Space(80);
            GUILayout.EndHorizontal();

            if (GUI.changed)
                EditorUtility.SetDirty(target);
            
        }

        private void DrawGenericTabs()
        {
            int index = (int)_genericCategorySelected;
            Data.Type = (GenericType)GUILayout.Toolbar(index, _genericCategoryLabels.ToArray());
            index = (int)Data.Type;
            _genericCategorySelected = (int)_genericCategories[index];         
        }

        void DrawItemTabs()
        {
            int index = (int)_itemCategorySelected;
            Data.SpecificItemType = (DumbProject.Items.ItemType)GUILayout.Toolbar(index, _itemCategoryLabels.ToArray());
            index = (int)Data.SpecificItemType;
            _itemCategorySelected = (int)_itemCategory[index];
        }

        void DrawEnemyTabs()
        {
            int index = (int)_enemyCategorySelected;
            Data.SpecificEnemyType = (DumbProject.Items.EnemyType)GUILayout.Toolbar(index, _enemyCategoryLabels.ToArray());
            index = (int)Data.SpecificEnemyType;
            _enemyCategorySelected = (int)_enemyCategory[index];
        }

        void DrawTrapsTabs()
        {
            int index = (int)_trapCategorySelected;
            Data.SpecificTrapType = (DumbProject.Items.TrapType)GUILayout.Toolbar(index, _trapCategoryLabels.ToArray());
            index = (int)Data.SpecificTrapType;
            _trapCategorySelected = (int)_trapCategory[index];
        }
    }
}