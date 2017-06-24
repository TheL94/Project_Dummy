using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items.ALB
{
    //Classe generica di Dati. Qui metti tutti i dati che servono agli item. Anche quelli che potrebbero essere inutili (come il danno, anche se sono gattini, etc...).
    //Se proprio vuoi puoi estendere questa classe facendo derivare degli altri scriptable tipo "MonsterData", "TrapData", ecc... Chiaramente devi un po' rivedere quanto scritto in ItemGeneric e ItemManager, ma nulla di assurdo.
    public class ItemData : ScriptableObject
    {
        public ItemType ItemType;
        public GameObject Model;
    }

    public enum ItemType
    {
        Dai,
        Fulvio,
        ceri,
        quasi
    }
}
