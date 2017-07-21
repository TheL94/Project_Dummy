using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    public abstract class CellElement : MonoBehaviour
    {
        [HideInInspector]
        public Cell RelativeCell;

        protected GameObject graphic;

        public virtual void Setup(Cell _relativeCell)
        {
            RelativeCell = _relativeCell;
        }

        /// <summary>
        /// Funzione che mette l'oggetto di grafica nella posizione giusta e lo mette figlio di CellElement
        /// </summary>
        /// <param name="_graphic"></param>
        /// <param name="_rotation"></param>
        public virtual void SetGraphic(GameObject _graphic, Quaternion _rotation)
        {
            graphic = _graphic;
            graphic.transform.position = transform.position;
            graphic.transform.rotation = _rotation;
            graphic.transform.parent = transform;
        }

        /// <summary>
        /// Funzione che disabilita l'oggetto
        /// </summary>
        public virtual void DisableObject(bool _avoidDestruction = false) { }
    }
}