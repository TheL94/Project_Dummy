﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.GDR;
using DumbProject.Rooms;

namespace DumbProject.Items
{
    public class RoomFillerManager : MonoBehaviour
    {
        #region Variables
        [HideInInspector] public List<ItemGenericData> Istances_itemsData; //lista di Weapons,Potions e Armors.
        [HideInInspector] public List<TrapData> Istances_gdr_data;// lista di Traps e TimeWaster.
        [HideInInspector] public List<GDR_Controller> Istances_Enemy;// lista di enemy.
        #endregion

        #region Setup
        public void Init(List<ItemGenericData> _Istances_itemsData, List<TrapData> _Istances_gdr_data)
        {
            Istances_gdr_data = _Istances_gdr_data;
            Istances_itemsData = _Istances_itemsData;
        } 
        #endregion

        #region API
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_room"></param>
        public void InstantiateObjectsInRoom(Room _room)
        {
            ItemGenericData genericaDroppableData = ChooseItem();
            if (genericaDroppableData != null)
            {
                IDroppable droppable = CreateIDroppable(genericaDroppableData);
                droppable.Data = genericaDroppableData;
                _room.AddInteractable(droppable); 
            }
            TrapData trapData = ChooseTraps();
            if (trapData != null)
            {
                CreateTrap(trapData, _room.CellsInRoom[Random.Range(0, _room.CellsInRoom.Count)]);
            }
        }

        /// <summary>
        /// Ritorna l'item da istanziare nella ui in base alla percentuale di spawn
        /// </summary>
        /// <returns></returns>
        ItemGenericData ChooseItem()
        {
            float percentage;
            float minValue = 0;
            float randNum = Random.Range(0f, 100f);

            foreach (ItemGenericData _data in Istances_itemsData)
            {
                percentage = _data.PercentageToSpawn;
                if (randNum > minValue && randNum <= (minValue + percentage))
                    return _data;
                else
                    minValue += percentage;
            }
            return null;
        }
        /// <summary>
        /// Ritorna l'item da istanziare nella ui in base alla percentuale di spawn
        /// </summary>
        /// <returns></returns>
        GDR_Controller ChoseEnemy()
        {
            float percentage;
            float minValue = 0;
            float randNum = Random.Range(0f, 100f);

            foreach (GDR_Controller controller in Istances_Enemy)
            {
                percentage = controller.Data.PercentageToSpawn;
                if (randNum > minValue && randNum <= (minValue + percentage))
                    return controller;
                else
                    minValue += percentage;
            }
            return null;
        }

        TrapData ChooseTraps()
        {
            float percentage;
            float minValue = 0;
            float randNum = Random.Range(0f, 100f);

            foreach (TrapData _data in Istances_gdr_data)
            {
                percentage = _data.PercentageToSpawn;
                if (randNum > minValue && randNum <= (minValue + percentage))
                    return _data;
                else
                    minValue += percentage;
            }
            return null;
        }
        /// <summary>
        /// A seconda del tipo del data entra all'interno del sottotipo e viene aggiunto il component corrispondente
        /// chiamandone anche l'init dove vengono passati i valori relativi all'oggetto
        /// </summary>
        /// <param name="_data">Il data relativo all'oggetto che viene istanziato</param>
        ItemGeneric CreateIDroppable(ItemGenericData _data)
        {
            GameObject newObj = new GameObject();
            ItemGeneric item = null;

            if (_data.GetType() == typeof(WeaponData))
            {
                item = newObj.AddComponent<Weapon>();
                item.name = _data.Name;
            }
            else if (_data.GetType() == typeof(PotionData))
            {
                item = newObj.AddComponent<Potion>();
                item.name = _data.Name;
            }
            else if (_data.GetType() == typeof(ArmorData))
            {
                item = newObj.AddComponent<Armor>();
                item.name = _data.Name;
            }

            Instantiate(_data.ItemPrefab, item.Transf.position, item.Transf.rotation, item.Transf);
            item.Init(_data);
            return item;
        }
        /// <summary>
        /// Creazione della trappola o Time Waster
        /// </summary>
        /// <param name="_trapData"></param>
        /// <param name="_cell"></param>
        /// <returns></returns>
        Trap CreateTrap(TrapData _trapData, Cell _cell) {

            Trap newTrap = Instantiate(_trapData.ItemPrefab, _cell.transform).AddComponent<Trap>();
            newTrap.transform.position = _cell.transform.position;
            newTrap.Init(_trapData, _cell);
            return newTrap;

        }
        #endregion
    }
}