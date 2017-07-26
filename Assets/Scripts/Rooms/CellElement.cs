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

        protected GameObject graphicObj;

        public virtual void Setup(Cell _relativeCell)
        {
            RelativeCell = _relativeCell;
        }

        /// <summary>
        /// Funzione che mette l'oggetto di grafica nella posizione giusta e lo mette figlio di CellElement
        /// </summary>
        /// <param name="_graphic"></param>
        /// <param name="_rotation"></param>
        public void SetGraphic(GameObject _graphic, Quaternion _rotation)
        {
            DisableGraphic();

            graphicObj = _graphic;
            graphicObj.transform.position = transform.position;
            graphicObj.transform.rotation = _rotation;
            graphicObj.transform.parent = transform;
        }

        /// <summary>
        /// Funzione che disabilita la grafica e la ritorna al pool
        /// </summary>
        public void DisableGraphic()
        {
            if (graphicObj != null)
            {
                graphicObj.SetActive(false);
                graphicObj = null;
                GameManager.I.PoolMng.UpdatePools();
            }
        }

        /// <summary>
        /// Funzione che disabilita l'oggetto
        /// </summary>
        public virtual void DisableAndDestroyObject(bool _destroyComponentOnly = false)
        {
            DisableGraphic();
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
                DestroyImmediate(gameObject);
        }
    }
}