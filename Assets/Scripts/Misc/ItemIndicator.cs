using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DumbProject.Items
{
    public class ItemIndicator : MonoBehaviour
    {
        Image ImagePrefab;
        IDroppable item;
        public bool inUI = true;
        Image image;

        RaycastHit hit;
        Ray ray;

        void Start()
        {
            ImagePrefab = (Resources.Load("Prefabs/Misc/ImagePrefab") as GameObject).GetComponent<Image>();         //Carica da resources il prefab dell'immagine da istanziare
            item = GetComponent<IDroppable>();                                      // Prende riferimento dell'IDroppable a cui è attaccato
            ray = new Ray(transform.position, Vector3.up);                          

            /// All'interno del room preview è presente un canvas attaccato alla camera che scatena la reazione del raycast.
            /// Istanzia l'immagine figlia dell'oggetto con cui entra in collisione(canvas), e setta la sprite con "ItemInUI" presente nel data contenuto nell'IDroppable 
            if (Physics.Raycast(ray, out hit, 13f))
            {
                image = Instantiate(ImagePrefab, hit.point, Quaternion.Euler(90, 0, 0), hit.collider.GetComponent<RectTransform>());
                image.sprite = item.Data.ItemInUI;
                image.color = SetImageColorFromTime(item.Data.TimeToSpend);
            }
            else inUI = false;

        }

        void Update()
        {
            if (inUI)
            {
                ray = new Ray(transform.position, Vector3.up);
                if (Physics.Raycast(ray, out hit, 13f))
                {
                    if (!image.gameObject.activeInHierarchy)
                        image.gameObject.SetActive(true);
                    image.transform.position = hit.point;
                }
                else
                {
                    image.gameObject.SetActive(false);
                } 
            }
        }

        /// <summary>
        /// Ritorna un colore in base al float passato come paramentro
        /// </summary>
        /// <param name="_time"></param>
        /// <returns></returns>
        Color SetImageColorFromTime(float _time)
        {
            if (_time >= 1 && _time <= 2)
                return Color.black; //Viola
            else if (_time >= 3 && _time <= 5)
                return Color.red;
            //else if (_time >= 6 && _time <= 8)
            //    return Color.orange;
            else if (_time >= 9 && _time <= 10)
                return Color.yellow;
            else if (_time >= 11 && _time <= 12)
                return Color.green;
            else
                return Color.white;
        }
    }
}