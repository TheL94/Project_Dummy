using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items {
    public class ItemManager : MonoBehaviour
    {
        //Puoi riempire questa lista a mano da editor o creare una funzione che carica gli script di Tipo ItemGeneric direttamente da risorse al setup
        public List<ItemLogic> ItemLogics = new List<ItemLogic>();
        //chiamata da chiunque si occupa di mettere in scena il gioco (dungeon o game manager per ora).
        public void Setup() { }
        //si occupa di far partire qualunque comportamento autonomo abbia ItemManager. Potrebbe essere accorpato a setup()
        public void Init() { }
        // Inserisce un oggetto di un certo tipo in scena, lo setuppa/inizializza e ritorna come valore l'oggetto (o null se per qualche mativo ha fallito l'istanziazione)
        public ItemGeneric PlaceItem(ItemData _data, Transform _positionAndRotation) {

            GameObject item = new GameObject("_data.nome");
            item.transform.position = _positionAndRotation.position;
            item.transform.rotation = _positionAndRotation.rotation;
            //Prima della logica metti la grafica in scena. In questa funzione è scritto come e dove.
            Instantiate(_data.Model, item.transform);




            //Si prende la logica da solo.
            ItemGeneric placedItem = item.AddComponent(GetItemLogicByType(_data.ItemType).GetType()) as ItemGeneric; //dovrebbe funzionare. Non sono 100% sicuro di questa riga.
            placedItem.Setup();
            placedItem.Init();

            return placedItem; // Se per qualche motivo non è possibile instanziare l'oggetto, ti fai ritornare null, così da sapere dov'è il problema.
        }

        //Viene chiamata per determinare la logica da usare relativamente al tipo.
        ItemGeneric GetItemLogicByType(ItemType _typeOfItem)
        {
            foreach (ItemLogic itemlogic in ItemLogics)
            {
                if (itemlogic.Type == _typeOfItem)
                    return itemlogic.LogicScript;
            }
            return null;
        }

        [System.Serializable]
        public struct ItemLogic
        {
            public ItemType Type;
            public ItemGeneric LogicScript;
        }
    }
}
