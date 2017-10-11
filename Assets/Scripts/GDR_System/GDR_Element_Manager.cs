using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.GDR;
using DumbProject.Generic;
using DumbProject.Rooms;

namespace DumbProject.GDR_System
{
    public class GDR_Element_Manager : MonoBehaviour
    {
        [HideInInspector] public List<GDR_Element_Generic_Data> Istances_GDR_Element_Data;

        #region Setup
        public void Init(List<GDR_Element_Generic_Data> _istances_GDR_Element_Data)
        {
            Istances_GDR_Element_Data = _istances_GDR_Element_Data;
        } 
        #endregion

        #region API
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_room"></param>
        public void InstantiateObjectsInRoom(Room _room)
        {
            GDR_Element_Generic_Data genericaDroppableData = ChooseElement();
            if (genericaDroppableData != null)
            {
                IDroppable droppable = CreateInteractable(genericaDroppableData);
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
        GDR_Element_Generic_Data ChooseElement()
        {
            float percentage;
            float minValue = 0;
            float randNum = Random.Range(0f, 100f);

            foreach (GDR_Element_Generic_Data _data in Istances_GDR_Element_Data)
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
        IInteractable CreateInteractable(GDR_Element_Generic_Data _data)
        {
            GameObject newObj = new GameObject();
            IInteractable item = null;

            if (_data.GetType() == typeof(WeaponData))
            {
                item = newObj.AddComponent<Weapon>();
                (item as Weapon).Init(_data);
            }
            else if (_data.GetType() == typeof(PotionData))
            {
                item = newObj.AddComponent<Potion>();
                (item as Potion).Init(_data);
            }
            else if (_data.GetType() == typeof(ArmorData))
            {
                item = newObj.AddComponent<Armor>();
                (item as Armor).Init(_data);
            }
            else if (_data.GetType() == typeof(TimeWasterData))
            {
                item = newObj.AddComponent<TimeWaster>();
                (item as TimeWaster).Init(_data);
            }
            else if (_data.GetType() == typeof(TrapData))
            {
                item = newObj.AddComponent<Trap>();
                (item as Trap).Init(_data);
            }
            //else if (_data.GetType() == typeof(EnemyData))
            //{
            //    item = newObj.AddComponent<Enemy>();
            //    (item as Enemy).Init(_data)
            //}

            Instantiate(_data.ElementPrefab, item.Transf.position, item.Transf.rotation, item.Transf);
            return item;
        }
        #endregion
    }
}