using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;


namespace DumbProject.Items
{
    public class ItemsManager : MonoBehaviour
    {
        List<ItemData> itemDatasCollection = new List<ItemData>();

        List<GenericDroppableData> AllDatas = new List<GenericDroppableData>();
        List<GenericDroppableData> itemDatas = new List<GenericDroppableData>();
        List<GenericDroppableData> enemyDatas = new List<GenericDroppableData>();
        List<GenericDroppableData> trapDatas = new List<GenericDroppableData>();
        List<GenericDroppableData> timeWasterDatas = new List<GenericDroppableData>();


        public void Init()
        {
            AllDatas = Resources.LoadAll<GenericDroppableData>("Data/ItemsData").ToList();

            foreach (GenericDroppableData _data in AllDatas)
            {
                if (_data.GetType() == typeof(WeaponData) || _data.GetType() == typeof(PotionData) || _data.GetType() == typeof(ArmorData))
                {
                    itemDatas.Add(Instantiate(_data));
                }else if(_data.GetType() == typeof(EnemyData))
                {
                    enemyDatas.Add(Instantiate(_data));
                }
                else if(_data.GetType() == typeof(TrapData))
                {
                    trapDatas.Add(Instantiate(_data));
                }
                else if(_data.GetType() == typeof(TimeWasterData))
                {
                    timeWasterDatas.Add(Instantiate(_data));
                }
            }

            CreateItemData();
        }

        /// <summary>
        /// Per ogni elemento contenuto nella lista AllDatas crea una nuova struttura ItemData con il data e il calcolo della sua percentuale di spawn
        /// </summary>
        void CreateItemData()
        {
            float percentageSum = 0;

            foreach (GenericDroppableData _data in AllDatas)
            {
                percentageSum += _data.PercentageToSpawn;
            }

            foreach (GenericDroppableData _data in AllDatas)
            {
                itemDatasCollection.Add(new ItemData()
                {
                    data = _data,
                    percentage = percentageSum != 0 ? ( _data.PercentageToSpawn * 100) / percentageSum : 0
                });
            }
        }

        /// <summary>
        /// Ritorna l'item da istanziare nella ui in base alla percentuale di spawn
        /// </summary>
        /// <returns></returns>
        GenericDroppableData ChooseItem()
        {
            float minValue = 0;
            float randNum = Random.Range(0f, 100f);

            foreach (ItemData _data in itemDatasCollection)
            {
                if (randNum > minValue && randNum <= (minValue + _data.percentage))
                    return _data.data;
                else
                    minValue += _data.percentage;
            }
            Debug.LogError("No Item to instantiate");
            return null;
        }

        public void InstantiateItemInRoom(Room _room)
        {
            GenericDroppableData data = ChooseItem();
            IDroppable droppable = CreateIDroppable(data);
            droppable.Data = data;
            _room.AddInteractable(droppable);
        }

        /// <summary>
        /// A seconda del tipo del data entra all'interno del sottotipo e viene aggiunto il component corrispondente
        /// chiamandone anche l'init dove vengono passati i valori relativi all'oggetto
        /// </summary>
        /// <param name="_data">Il data relativo all'oggetto che viene istanziato</param>
        ItemGeneric CreateIDroppable(GenericDroppableData _data)
        {
            GameObject newObj = new GameObject();
            ItemGeneric item = null;

            if (_data.GetType() == typeof(EnemyData))
            {
                switch ((_data as GenericDroppableData).SpecificEnemyType)
                {
                    case EnemyType.None:
                        break;
                    case EnemyType.Dragon:
                        // AddComponente<Drago>().Init((Data as GenericData).DragonDataValues)
                        break;
                    case EnemyType.Goblin:
                        break;
                }
            }
            else if (_data.GetType() == typeof(WeaponData))
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
            else if (_data.GetType() == typeof(TrapData))
            {
                
            }
            else if (_data.GetType() == typeof(TimeWasterData))
            {
                switch ((_data as GenericDroppableData).SpecificTrapType)
                {
                    case TrapType.None:
                        break;
                    case TrapType.Type1:
                        break;
                    case TrapType.Type2:
                        break;
                }
            }

            Instantiate(_data.ItemPrefab, item.Transf.position, item.Transf.rotation, item.Transf);
            item.Init(_data);
            return item;
        }
    }

    /// <summary>
    /// Struttura che contiente il data e la relativa percentuale (usata per la scelta dell'item con percentuale di spawn)
    /// </summary>
    struct ItemData
    {
        public GenericDroppableData data;
        public float percentage;
    }
}