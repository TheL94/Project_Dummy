using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DumbProject.Items;

namespace DumbProject.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GenericData))]
    public class CustomDataTypeEditor : Editor
    {
        GenericData Data;
        //PotionData potionData;
        //ArmoryData armoryData;

        private void OnEnable()
        {
            Data = (GenericData)target;
        }
        private List<GenericType> _genericCategories;
        List<ItemType> _itemCategory;
        List<EnemyType> _enemyCategory;

        private List<string> _genericCategoryLabels;
        private List<string> _itemCategoryLabels;
        private List<string> _enemyCategoryLabels;  

        int _genericCategorySelected = 0;
        int _itemCategorySelected = 0;
        int _enemyCategorySelected = 0;

        Sprite ItemSprite;
        GameObject prefab;
        Material material;


        private void InitCategories() { 
            _genericCategories = GetListFromEnum<GenericType>();
            _itemCategory = GetListFromEnum<ItemType>();
            _enemyCategory = GetListFromEnum<EnemyType>();

            _genericCategoryLabels = new List<string>();
            _itemCategoryLabels = new List<string>();
            _enemyCategoryLabels = new List<string>();
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

            if (_genericCategorySelected == 1)
            {
                DrawItemTabs();
                if(_itemCategorySelected == 0)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Damage");
                    Data.SwordDataValues.Damage = EditorGUILayout.FloatField(Data.SwordDataValues.Damage);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Wear");
                    Data.SwordDataValues.Wear = EditorGUILayout.FloatField(Data.SwordDataValues.Wear);
                    GUILayout.EndHorizontal();
                }

                if (_itemCategorySelected == 1)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Damage Hero");
                    Data.PotionDataValues.DamageHero = EditorGUILayout.FloatField(Data.PotionDataValues.DamageHero);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Healt Restore");
                    Data.PotionDataValues.HealtRestore = EditorGUILayout.FloatField(Data.PotionDataValues.HealtRestore);
                    GUILayout.EndHorizontal();
                }

                if (_itemCategorySelected == 2)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Protection");
                    Data.ArmoryDataValues.Protection = EditorGUILayout.FloatField(Data.ArmoryDataValues.Protection);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Wear");
                    Data.ArmoryDataValues.Wear = EditorGUILayout.FloatField(Data.ArmoryDataValues.Wear);
                    GUILayout.EndHorizontal();
                }

            }


            GUILayout.Space(15);
            //The sprite to show in the Inventory
            GUILayout.BeginHorizontal();
            Data.ItemInUI = (Sprite)EditorGUILayout.ObjectField("Sprite for Invetory", ItemSprite, typeof(Sprite), false);
            GUILayout.EndHorizontal();

            // The prefab to instantiate
            GUILayout.BeginHorizontal();
            Data.ItemPrefab = (GameObject)EditorGUILayout.ObjectField("ItemPrefab", prefab, typeof(GameObject), false);
            GUILayout.EndHorizontal();

            //The Material to apply at the floor in the room
            GUILayout.BeginHorizontal();
            Data.ShowMateriaInRoom = (Material)EditorGUILayout.ObjectField("Material For Room", material, typeof(Material), false);
            GUILayout.EndHorizontal();

            //base.OnInspectorGUI();
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
            Data.SpecificType = (DumbProject.Items.ItemType)GUILayout.Toolbar(index, _itemCategoryLabels.ToArray());
            index = (int)Data.SpecificType;
            _itemCategorySelected = (int)_itemCategory[index];
        }

        void DrawEnemyTabs()
        {

        }

    }
}