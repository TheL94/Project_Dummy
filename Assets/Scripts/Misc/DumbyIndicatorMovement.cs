using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using UnityEngine.UI;

namespace DumbProject.UI
{
    public class DumbyIndicatorMovement : MonoBehaviour
    {
        UIManager uiManager;
        public GameObject DumbyIconPrefab;
        GameObject DumbyIcon;

        private void Start()
        {
            uiManager = GameManager.I.UIMng;
            Vector3 vector = Camera.main.WorldToScreenPoint(transform.position) ;
            if (uiManager != null) {
                DumbyIcon = Instantiate(DumbyIconPrefab, vector + Vector3.up * 100, Quaternion.identity, uiManager.transform).gameObject;
            }
        }

        private void Update()
        {
            UpdateIndicatorPosition();
            if (GameManager.I.CurrentState == Flow.FlowState.Pause)
                DumbyIcon.GetComponent<Image>().enabled = false;
            else if (GameManager.I.CurrentState == Flow.FlowState.Gameplay)
                DumbyIcon.GetComponent<Image>().enabled = true;
        }

        /// <summary>
        /// Setta la posizione dell'indicare in UI
        /// </summary>
        void UpdateIndicatorPosition()
        {
            DumbyIcon.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 100;
        }

    }
}