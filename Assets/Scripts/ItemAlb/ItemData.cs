using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    //Classe generica di Dati. Qui metti tutti i dati che servono agli item. Anche quelli che potrebbero essere inutili (come il danno, anche se sono gattini, etc...).
    //Se proprio vuoi puoi estendere questa classe facendo derivare degli altri scriptable tipo "MonsterData", "TrapData", ecc... Chiaramente devi un po' rivedere quanto scritto in ItemGeneric e ItemManager, ma nulla di assurdo.
    public class ItemData : ScriptableObject
    {
        public ItemType ItemType;
        public GameObject Model;
        public ItemGeneric ItemLogic { get { return GetItemLogicByType(ItemType); } }

        //Property che una sola volta per uso ti cerca la lista di tutti gli script degli item e te la salva per eventuali usi futuri.
        List<ItemGeneric> _itemLogics;
        protected List<ItemGeneric> itemLogics { get {
                if(_itemLogics == null)
                    _itemLogics = Resources.LoadAll<ItemGeneric>("PERCORSO DI DIRECTORY DOVE CI SONO TUTTI GLI SCRIPT DEGLI ITEM CHE DERIVANO DA ItemGeneric").ToList();
                return _itemLogics;
            }
        }

        //Viene chiamata al Get di ItemLogic (property) per determinare la logica da usare relativamente al tipo.
        ItemGeneric GetItemLogicByType(ItemType _typeOfItem)
        {
            foreach (ItemGeneric itemlogic in itemLogics)
            {
                if (itemlogic.Type == _typeOfItem)
                    return itemlogic;
            }
            return null;
        }
    }

    public enum ItemType
    {
        Dai,
        Fulvio,
        ceri,
        quasi
    }
}
