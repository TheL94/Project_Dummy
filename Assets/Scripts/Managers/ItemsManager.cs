using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms.Cells;


namespace DumbProject
{
    public class ItemsManager : MonoBehaviour
    {
        public List<ItemBaseData> ItemsData = new List<ItemBaseData>();


        /// <summary>
        /// Ritorna l'item da istanziare nella ui
        /// </summary>
        /// <returns></returns>
        ItemBaseData ChooseItem()
        {
            int randNum = Random.Range(0, ItemsData.Count);
            return ItemsData[randNum];
        }


        public void InstantiateItemInRoom(Cell _cell)
        {
            Instantiate(ChooseItem().ItemPrefab, _cell.RelativeNode.WorldPosition + new Vector3(0, 2, 0), Quaternion.identity, _cell.transform);
            _cell.IsFree = false;
        }
    }
}