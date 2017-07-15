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
            ImagePrefab = (Resources.Load("Prefabs/Misc/ImagePrefab") as GameObject).GetComponent<Image>();
            item = GetComponent<IDroppable>();
            ray = new Ray(transform.position, Vector3.up);

            if (Physics.Raycast(ray, out hit, 13f))
            {
                image = Instantiate(ImagePrefab, hit.point, Quaternion.Euler(90, 0, 0), hit.collider.GetComponent<RectTransform>());
                image.sprite = item.Data.ItemInUI;
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
    }
}