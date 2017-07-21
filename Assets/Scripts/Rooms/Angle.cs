using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    public class Angle : CellElement
    {
        /// <summary>
        /// Funzione che disabilita l'oggetto
        /// </summary>
        /// <param name="_avoidDestruction"></param>
        public override void DisableObject(bool _avoidDestruction = false)
        {
            if (graphic != null)
            {
                graphic.SetActive(false);
                graphic = null;
            }
            RelativeCell.Angles.Remove(this);
            GameManager.I.PoolMng.UpdatePools();

            if (!_avoidDestruction)
                Destroy(gameObject);
            else
                Destroy(this);
        }
    }
}