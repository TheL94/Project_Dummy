using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;


namespace DumbProject.Items
{
    public class ItemsManager : MonoBehaviour
    {
        public List<DroppableBaseData> AllDatas = new List<DroppableBaseData>();

        List<DroppableBaseData> itemDatas = new List<DroppableBaseData>();
        List<DroppableBaseData> enemyDatas = new List<DroppableBaseData>();
        List<DroppableBaseData> trapDatas = new List<DroppableBaseData>();
        List<DroppableBaseData> gattiniDatas = new List<DroppableBaseData>();

        public void Init()
        {
            foreach (DroppableBaseData _data in AllDatas)
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

                    case GenericType.Gattini:
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
        DroppableBaseData ChooseItem()
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
            DroppableBaseData tempData = ChooseItem();
            if(tempData.ItemPrefab.GetComponent<IDroppable>() != null)
            {
                _room.AddDroppable(tempData).InitIDroppable(tempData);
            }
        }

        public void InstantiateItemInRoom(Room _room, DroppableBaseData _data)
        {
            if (_data.ItemPrefab.GetComponent<IDroppable>() != null)
            {
                _room.AddDroppable(_data);
            }
        }

    }
}