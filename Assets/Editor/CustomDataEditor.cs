﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DumbProject.Items;

namespace DumbProject.Editors
{
    [System.Serializable]
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GenericItemData))]
    public class CustomDataTypeEditor : Editor
    {
        GenericItemData Data;
        

        private void OnEnable()
        {
            Data = (GenericItemData)target;
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
            DrawGenericTabs();

            //data.Type = (DumbProject.Items.GenericType)EditorGUILayout.EnumPopup("Generic Type", data.Type);

            ///Enemies
            if(_genericCategorySelected == 0)
            {
                DrawEnemyTabs();
                //_genericCategorySelected = 0;
                if (_enemyCategorySelected == 1)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Damage");
                    GUILayout.Space(100);
                    Data.DragonDataValues.Damage = EditorGUILayout.FloatField(Data.DragonDataValues.Damage);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Life");
                    GUILayout.Space(100);
                    Data.DragonDataValues.Life = EditorGUILayout.FloatField(Data.DragonDataValues.Life);
                    GUILayout.EndHorizontal();
                }

                if (_enemyCategorySelected == 2)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Damage");
                    GUILayout.Space(100);
                    Data.GoblinDataValues.Damage = EditorGUILayout.FloatField(Data.GoblinDataValues.Damage);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Life");
                    GUILayout.Space(100);
                    Data.GoblinDataValues.Life = EditorGUILayout.FloatField(Data.GoblinDataValues.Life);
                    GUILayout.EndHorizontal();
                }
            }

            ///Items
            if (_genericCategorySelected == 1)
            {
                DrawItemTabs();
                //_genericCategorySelected = 1;
                if (_itemCategorySelected == 1)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Damage");
                    GUILayout.Space(100);
                    Data.SwordDataValues.Damage = EditorGUILayout.FloatField(Data.SwordDataValues.Damage);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Wear");
                    GUILayout.Space(100);
                    Data.SwordDataValues.Wear = EditorGUILayout.FloatField(Data.SwordDataValues.Wear);
                    GUILayout.EndHorizontal();
                }

                if (_itemCategorySelected == 2)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Damage Hero");
                    GUILayout.Space(100);
                    Data.PotionDataValues.DamageHero = EditorGUILayout.FloatField(Data.PotionDataValues.DamageHero);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Healt Restore");
                    GUILayout.Space(100);
                    Data.PotionDataValues.HealtRestore = EditorGUILayout.FloatField(Data.PotionDataValues.HealtRestore);
                    GUILayout.EndHorizontal();
                }

                if (_itemCategorySelected == 3)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Protection");
                    GUILayout.Space(100);
                    Data.ArmoryDataValues.Protection = EditorGUILayout.FloatField(Data.ArmoryDataValues.Protection);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Wear");
                    GUILayout.Space(100);
                    Data.ArmoryDataValues.Wear = EditorGUILayout.FloatField(Data.ArmoryDataValues.Wear);
                    GUILayout.EndHorizontal();
                }

            }

            ///Traps
            if(_genericCategorySelected == 2)
            {
                DrawTrapsTabs();

                //_genericCategorySelected = 2;

                if (_enemyCategorySelected == 1)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Damage");
                    GUILayout.Space(100);
                    Data.TagliolaValues.Damage = EditorGUILayout.FloatField(Data.TagliolaValues.Damage);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Time To Leave");
                    GUILayout.Space(100);
                    Data.TagliolaValues.TimeToLeave = EditorGUILayout.FloatField(Data.TagliolaValues.TimeToLeave);
                    GUILayout.EndHorizontal();
                }

                if (_enemyCategorySelected == 2)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Damage");
                    GUILayout.Space(100);
                    Data.CatapultaValues.Damage = EditorGUILayout.FloatField(Data.CatapultaValues.Damage);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Time To Leave");
                    GUILayout.Space(100);
                    Data.CatapultaValues.TimeToLeave = EditorGUILayout.FloatField(Data.CatapultaValues.TimeToLeave);
                    GUILayout.EndHorizontal();
                }
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

            if (GUI.changed)
                EditorUtility.SetDirty(target);
            
        }

        private void DrawGenericTabs() {
            int index = (int)_genericCategorySelected;
            Data.Type = (DumbProject.Items.GenericType)GUILayout.Toolbar(index, _genericCategoryLabels.ToArray());
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