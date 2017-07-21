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
            DisableGraphic();
            RelativeCell.Angles.Remove(this);

            if (!_avoidDestruction)
                Destroy(gameObject);
            else
                Destroy(this);
        }
    }
}