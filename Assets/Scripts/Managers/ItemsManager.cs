using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;


namespace DumbProject.Items
{
    public class ItemsManager : MonoBehaviour
    {
        public List<GenericItemData> AllDatas = new List<GenericItemData>();

        List<GenericItemData> itemDatas = new List<GenericItemData>();
        List<GenericItemData> enemyDatas = new List<GenericItemData>();
        List<GenericItemData> trapDatas = new List<GenericItemData>();
        List<GenericItemData> gattiniDatas = new List<GenericItemData>();

        public void Init()
        {
            foreach (GenericItemData _data in AllDatas)
            {
                switch (_data.Type)
                {
                    case GenericType.Item:
                        itemDatas.Add(Instantiate(_data));
                        break;

                    case GenericType.Ememy:
                        enemyDatas.Add(Instantiate(_data));
                        break;

                    case GenericType.Trap:
                        trapDatas.Add(Instantiate(_data));
                        break;

                    case GenericType.TimeWaster:
                        gattiniDatas.Add(Instantiate(_data));
                        break;
                    default:
                        itemDatas.Add(Instantiate(_data));
                        break;
                }
            }
        }

        /// <summary>
        /// Ritorna l'item da istanziare nella ui
        /// </summary>
        /// <returns></returns>
        GenericItemData ChooseItem()
        {
            int randNum = Random.Range(0, itemDatas.Count);
            return itemDatas[randNum];
        }

        /// <summary>
        /// Sceglie il tipo di oggetto da istanziare nella stanza
        /// </summary>
        GenericType ChoseTypeToSpawn()
        {
            return GenericType.Item;
        }

        public void InstantiateItemInRoom(Room _room)
        {
            GenericItemData data = ChooseItem();
            _room.AddInteractable(CreateIDroppable(data));
        }

        /// <summary>
        /// A seconda del tipo del data entra all'interno del sottotipo e viene aggiunto il component corrispondente
        /// chiamandone anche l'init dove vengono passati i valori relativi all'oggetto
        /// </summary>
        /// <param name="_data">Il data relativo all'oggetto che viene istanziato</param>
        ItemGeneric CreateIDroppable(GenericItemData _data)
        {
            GameObject newObj = new GameObject();
            ItemGeneric item = null;
            switch (_data.Type)
            {
                /// Da completare con l'aggiunta del component corrispondente e la chiamata all'init passando i Values del data
                case GenericType.Ememy:
                    switch ((_data as GenericItemData).SpecificEnemyType)
                    {
                        case EnemyType.None:
                            break;
                        case EnemyType.Dragon:
                            // AddComponente<Drago>().Init((Data as GenericData).DragonDataValues)
                            break;
                        case EnemyType.Goblin:
                            break;
                    }
                    break;
                case GenericType.Item:
                    switch ((_data as GenericItemData).SpecificItemType)
                    {
                        case ItemType.None:
                            break;
                        case ItemType.Weapon:
                            item = newObj.AddComponent<Weapon>();
                            item.name = "Weapon";
                            break;
                        case ItemType.Potion:
                            item = newObj.AddComponent<Potion>();
                            item.name = "Potion";
                            break;
                        case ItemType.Armor:
                            item = newObj.AddComponent<Weapon>();
                            item.name = "Armor";
                            break;
                    }
                    break;
                /// Da completare con l'aggiunta del component corrispondente e la chiamata all'init passando i Values del data
                case GenericType.Trap:
                    switch ((_data as GenericItemData).SpecificTrapType)
                    {
                        case TrapType.None:
                            break;
                        case TrapType.Type1:
                            break;
                        case TrapType.Type2:
                            break;
                    }
                    break;
                case GenericType.TimeWaster:
                    break;
            }
            Instantiate(_data.ItemPrefab, item.Transf);
            item.Init(_data);
            return item;
        }
    }
}