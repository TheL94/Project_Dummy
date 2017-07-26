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
        public override void DisableAndDestroyObject(bool _avoidDestruction = false)
        {
            RelativeCell.Angles.Remove(this);
            base.DisableAndDestroyObject(_avoidDestruction);
        }
    }
}