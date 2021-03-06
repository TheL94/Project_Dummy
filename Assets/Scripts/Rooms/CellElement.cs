﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    public abstract class CellElement : MonoBehaviour
    {
        [HideInInspector]
        public Cell RelativeCell;

        protected GameObject _graphicElement;
        public GameObject GraphicElement { get { return _graphicElement; } private set { _graphicElement = value; } }
        protected List<GameObject> fillerGraphics = new List<GameObject>();
        protected List<GameObject> lightingObjects = new List<GameObject>();

        public virtual void Setup(Cell _relativeCell)
        {
            RelativeCell = _relativeCell;
        }

        private void OnBecameInvisible()
        {
            DisableAllGraphics();
        }

        #region Graphic Element
        /// <summary>
        /// Funzione che mette l'oggetto di grafica nella posizione giusta e lo mette figlio di CellElement
        /// </summary>
        /// <param name="_graphic"></param>
        /// <param name="_rotation"></param>
        public virtual void SetGraphicElement(GameObject _graphic, Quaternion _rotation)
        {
            DisableGraphicElement();

            GraphicElement = _graphic;
            GraphicElement.transform.position = transform.position;
            GraphicElement.transform.rotation = _rotation;
            GraphicElement.transform.parent = transform;
        }

        /// <summary>
        /// Funzione che disabilita la grafica e la ritorna al pool
        /// </summary>
        public virtual void DisableGraphicElement()
        {
            if (GraphicElement != null)
            {
                GraphicElement.SetActive(false);
                GraphicElement = null;
                GameManager.I.PoolMng.UpdatePools();
            }
        }
        #endregion

        #region Filler Graphic
        /// <summary>
        /// Funzione che disabilita tutti gli oggetti di grafica riempitiva
        /// </summary>
        public virtual void DisableAllFillerGraphic()
        {
            for (int i = 0; i < fillerGraphics.Count; i++)
            {
                if (fillerGraphics[i] != null)
                {
                    fillerGraphics[i].SetActive(false);
                }
            }
            fillerGraphics.Clear();
            GameManager.I.PoolMng.UpdatePools();
        }

        /// <summary>
        /// Funzione che aggiunge un nuovo oggetto di grafica riempitiva a questo elemento
        /// </summary>
        /// <param name="_graphic"></param>
        /// <param name="_position"></param>
        /// <param name="_rotation"></param>
        public virtual void SetFillerGraphic(GameObject _graphic, Vector3 _position, Quaternion _rotation)
        {
            _graphic.transform.position = _position;
            _graphic.transform.rotation = _rotation;
            SetFillerGraphic(_graphic);
        }

        /// <summary>
        /// Funzione che aggiunge un nuovo oggetto di grafica riempitiva a questo elemento
        /// </summary>
        /// <param name="_graphic"></param>
        public virtual void SetFillerGraphic(GameObject _graphic)
        {
            _graphic.transform.parent = transform;
            fillerGraphics.Add(_graphic);
        }

        /// <summary>
        /// Cambia il parent degli ogetti di grafica riempitiva lasciandoli nella stessa posizione
        /// </summary>
        /// <param name="_newParent"></param>
        public void SwapFillerGraphicParent(CellElement _newParent)
        {
            for (int i = 0; i < fillerGraphics.Count; i++)
            {
                _newParent.SetFillerGraphic(fillerGraphics[i]);
            }
            fillerGraphics.Clear();
        }

        /// <summary>
        /// Ritorna TRUE se l'oggeto ha come figli degli oggetti di grafica riempitiva, altrimenti FALSE
        /// </summary>
        /// <returns></returns>
        public bool HasFillerGraphic()
        {
            if (fillerGraphics.Count == 0)
                return false;
            else
                return true;
        }
        #endregion

        #region Lighting Object
        /// <summary>
        /// Funzione che disabilita tutti gli oggetti di illuminazione
        /// </summary>
        public void DisableAllLightingObject()
        {
            for (int i = 0; i < lightingObjects.Count; i++)
            {
                if (lightingObjects[i] != null)
                {
                    lightingObjects[i].SetActive(false);
                }
            }
            lightingObjects.Clear();
            GameManager.I.PoolMng.UpdatePools();
        }

        /// <summary>
        /// Aggiunge un oggetto di illuminazione figlio di questo elemento
        /// </summary>
        /// <param name="_lightingObj"></param>
        /// <param name="_position"></param>
        public virtual void SetLightingObject(GameObject _lightingObj, Vector3 _position)
        {
            _lightingObj.transform.position = _position;
            SetLightingObject(_lightingObj);
        }

        /// <summary>
        /// Aggiunge un oggetto di illuminazione figlio di questo elemento
        /// </summary>
        /// <param name="_lightingObj"></param>
        public virtual void SetLightingObject(GameObject _lightingObj)
        {
            _lightingObj.transform.parent = transform;
            lightingObjects.Add(_lightingObj);
        }

        /// <summary>
        /// Cambia il parent degli ogetti di illuminazione lasciandoli nella stessa posizione
        /// </summary>
        /// <param name="_newParent"></param>
        public void SwapLightingObjectParent(CellElement _newParent)
        {
            for (int i = 0; i < fillerGraphics.Count; i++)
            {
                _newParent.SetFillerGraphic(fillerGraphics[i]);
            }
            fillerGraphics.Clear();
        }

        /// <summary>
        /// Ritorna TRUE se l'oggeto ha come figli degli oggetti di illuminazione, altrimenti FALSE 
        /// </summary>
        /// <returns></returns>
        public bool HasLightingObject()
        {
            if (lightingObjects.Count == 0)
                return false;
            else
                return true;
        }
        #endregion

        public virtual void DisableAllGraphics()
        {
            DisableGraphicElement();
            DisableAllFillerGraphic();
            DisableAllLightingObject();
        }

        /// <summary>
        /// Funzione che distrugge l'oggetto
        /// </summary>
        /// <param name="_destroyComponentOnly"></param>
        public virtual void DestroyObject(bool _destroyComponentOnly = false)
        {
            if (_destroyComponentOnly)
                Destroy(this);
            else
                Destroy(gameObject);
        }
    }
}