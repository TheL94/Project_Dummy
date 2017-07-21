using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    public class Floor : CellElement
    {
        /// <summary>
        /// Funzione che disabilita l'oggetto
        /// </summary>
        public override void DisableObject(bool _avoidDestruction = false)
        {
            if (graphic != null)
            {
                graphic.SetActive(false);
                graphic = null;
            }
            RelativeCell.Floor = null;
            GameManager.I.PoolMng.UpdatePools();

            if (!_avoidDestruction)
                Destroy(gameObject);
            else
                Destroy(this);
        }
    }
}

