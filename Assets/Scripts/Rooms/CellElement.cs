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

        protected GameObject graphicElement;
        protected GameObject fillerGraphic;

        public virtual void Setup(Cell _relativeCell)
        {
            RelativeCell = _relativeCell;
        }

        /// <summary>
        /// Funzione che mette l'oggetto di grafica nella posizione giusta e lo mette figlio di CellElement
        /// </summary>
        /// <param name="_graphic"></param>
        /// <param name="_rotation"></param>
        public virtual void SetGraphicElement(GameObject _graphic, Quaternion _rotation)
        {
            DisableGraphicElement();

            graphicElement = _graphic;
            graphicElement.transform.position = transform.position;
            graphicElement.transform.rotation = _rotation;
            graphicElement.transform.parent = transform;
        }

        /// <summary>
        /// Funzione che disabilita la grafica e la ritorna al pool
        /// </summary>
        public virtual void DisableGraphicElement()
        {
            if (graphicElement != null)
            {
                graphicElement.SetActive(false);
                graphicElement = null;
                GameManager.I.PoolMng.UpdatePools();
            }

            if (fillerGraphic != null)
            {
                fillerGraphic.SetActive(false);
                fillerGraphic = null;
                GameManager.I.PoolMng.UpdatePools();
            }
        }

        public virtual void SetFillerGraphic(GameObject _graphic, Vector3 _position, Quaternion _rotation)
        {
            fillerGraphic = _graphic;
            fillerGraphic.transform.position = _position;
            fillerGraphic.transform.rotation = _rotation;
            fillerGraphic.transform.parent = transform;
        }

        /// <summary>
        /// Funzione che disabilita l'oggetto
        /// </summary>
        public virtual void DisableAndDestroyObject(bool _destroyComponentOnly = false)
        {
            DisableGraphicElement();
            DestroyObject(_destroyComponentOnly);
        }

        /// <summary>
        /// Funzione che distrugge l'oggetto
        /// </summary>
        /// <param name="_destroyComponentOnly"></param>
        public void DestroyObject(bool _destroyComponentOnly = false)
        {
            if (_destroyComponentOnly)
                Destroy(this);
            else
                Destroy(gameObject);
        }
    }
}