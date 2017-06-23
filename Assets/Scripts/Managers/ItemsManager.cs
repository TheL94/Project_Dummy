using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms.Cells;
using DumbProject.Items;


namespace DumbProject.Generic
{
    public class ItemsManager : MonoBehaviour
    {
        // "Prefab"
        public List<BaseData> ItemsData = new List<BaseData>();

        // Dati effettivi
        List<BaseData> ItemsDataInstances = new List<BaseData>();

        public void Init()
        {

        }

        public void InstantiateDataInRoom(Cell _cell)
        {
            //Instantiate(ChooseItem().Prefab, _cell.RelativeNode.WorldPosition + new Vector3(0, 2, 0), Quaternion.identity, _cell.transform);
            //_cell.IsFree = false;

            BaseData data = ChooseItem();
            GameObject newObj = new GameObject();

        }

        void InstaciateData()
        {
            foreach (BaseData data in ItemsData)
            {
                ItemsDataInstances.Add(Instantiate(data));
            }
        }

        /// <summary>
        /// Ritorna l'item da istanziare nella ui
        /// </summary>
        /// <returns></returns>
        BaseData ChooseItem()
        {
            int randNum = Random.Range(0, ItemsDataInstances.Count);
            return ItemsDataInstances[randNum];
        }


        void AddDroppableScript(BaseData _data, GameObject _obj)
        {
            switch (_data.Type)
            {
                case BaseType.Item:
                    /*IDroppable Item =*/ AddItem((ItemData)_data, _obj);
                    break;
                case BaseType.Enemy:
                    break;
                case BaseType.Interactable:
                    break;
                default:
                    break;
            }
        }

        void AddItem(ItemData _data, GameObject _obj)
        {
            switch (_data.SpecificType)
            {
                case ItemType.Sward:
                    //_obj.AddComponent<Sward>();
                    break;
                case ItemType.Potion:
                    break;
                default:
                    break;
            }
        }

    }
}