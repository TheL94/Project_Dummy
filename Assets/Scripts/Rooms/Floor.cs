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
        /// <param name="_avoidDestruction"></param>
        public override void DisableObject(bool _avoidDestruction = false)
        {
            DisableGraphic();
            RelativeCell.Floor = null;

            if (!_avoidDestruction)
                Destroy(gameObject);
            else
                Destroy(this);
        }
    }
}

