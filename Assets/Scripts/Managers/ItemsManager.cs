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
        List<GenericItemData> timeWasterDatas = new List<GenericItemData>();

        public void Init()
        {
            // TODO : questo metodo fa piantare in modo irrimediabile la versione buildata.(righe 24 o 29)
            // Possibile errore nel tipe di scriptable 
            foreach (GenericItemData _data in AllDatas)
            {
                //if (_data.GetType() == typeof(EnemyData))
                //{
                //    enemyDatas.Add(Instantiate(_data));
                //}
                //else 
                if (_data.GetType() == typeof(WeaponData) || _data.GetType() == typeof(PotionData) || _data.GetType() == typeof(ArmorData))
                {
                    itemDatas.Add(Instantiate(_data));
                }
                //else if (_data.GetType() == typeof(TrapData))
                //{
                //    trapDatas.Add(Instantiate(_data));
                //}
                //else if (_data.GetType() == typeof(TimeWasterData))
                //{
                //    timeWasterDatas.Add(Instantiate(_data));
                //}
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
            IDroppable droppable = CreateIDroppable(data);
            droppable.Data = data;
            _room.AddInteractable(droppable);
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

            if (_data.GetType() == typeof(EnemyData))
            {
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
            }
            else if (_data.GetType() == typeof(WeaponData))
            {
                item = newObj.AddComponent<Weapon>();
                newObj.AddComponent<ItemIndicator>();
                item.name = "Weapon";
            }
            else if (_data.GetType() == typeof(PotionData))
            {
                item = newObj.AddComponent<Potion>();
                newObj.AddComponent<ItemIndicator>();
                item.name = "Potion";
            }
            else if (_data.GetType() == typeof(ArmorData))
            {
                item = newObj.AddComponent<Armor>();
                newObj.AddComponent<ItemIndicator>();
                item.name = "Armor";
            }
            else if (_data.GetType() == typeof(TrapData))
            {
                
            }
            else if (_data.GetType() == typeof(TimeWasterData))
            {
                switch ((_data as GenericItemData).SpecificTrapType)
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
}