using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms.Cells;


namespace DumbProject
{
    public class ItemsManager : MonoBehaviour
    {
        public List<BaseData> AllDatas = new List<BaseData>();

        List<BaseData> itemDatas = new List<BaseData>();
        List<BaseData> enemyDatas = new List<BaseData>();
        List<BaseData> trapDatas = new List<BaseData>();
        List<BaseData> gattiniDatas = new List<BaseData>();

        public void Init()
        {
            foreach (BaseData _data in AllDatas)
            {
                switch (_data.Type)
                {
                    case ItemType.Item:
                        itemDatas.Add(Instantiate(_data));
                        break;

                    case ItemType.Ememy:
                        enemyDatas.Add(Instantiate(_data));
                        break;

                    case ItemType.Trap:
                        trapDatas.Add(Instantiate(_data));
                        break;

                    case ItemType.Gattini:
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
        BaseData ChooseItem()
        {
            int randNum = Random.Range(0, itemDatas.Count);
            return itemDatas[randNum];
        }

        int i = 0;
        public void InstantiateItemInRoom(Cell _cell)
        {
            i++;
            Debug.Log("Oggetto istanziato " + i);
            BaseData temData = ChooseItem();
            IDroppable itemDropped = Instantiate(temData.ItemPrefab, _cell.RelativeNode.WorldPosition + new Vector3(0, 2, 0), Quaternion.identity, _cell.transform).GetComponent<IDroppable>();
            itemDropped.GetMyData(temData);
            _cell.IsFree = false;
        }
    }
}