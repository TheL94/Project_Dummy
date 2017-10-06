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
        Rect screenRect;

        bool dumbyIsVisibile = true;

        Vector3 lastVaiablePosition;

        private void Start()
        {
            screenRect = new Rect(0f, 0f, Screen.width, Screen.height);
            uiManager = GameManager.I.UIMng;
            Vector3 vector = Camera.main.WorldToScreenPoint(transform.position) ;
            if (uiManager != null) {
                DumbyIcon = Instantiate(DumbyIconPrefab, vector + Vector3.up * 70, Quaternion.identity, uiManager.transform);
            }
        }

        private void Update()
        {
            UpdateIndicatorPosition();
            if (GameManager.I.CurrentState == Flow.FlowState.Pause)
                DumbyIcon.GetComponent<Image>().enabled = false;
            else if (GameManager.I.CurrentState == Flow.FlowState.Gameplay)
                DumbyIcon.GetComponent<Image>().enabled = true;
            else if (GameManager.I.CurrentState == Flow.FlowState.RecapGame)
                Destroy(DumbyIcon);
        }

        /// <summary>
        /// Setta la posizione dell'indicare in UI
        /// </summary>
        void UpdateIndicatorPosition()
        {
            //lastVaiablePosition = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 100;
            if (dumbyIsVisibile)
            {
                DumbyIcon.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 70;
                if (CheckIfInsideTheScreen())
                    lastVaiablePosition = DumbyIcon.transform.position;
            }

            if (GetComponentInParent<Renderer>().isVisible)
            {
                dumbyIsVisibile = true;
            }
            else
            {
                dumbyIsVisibile = false;
            }
            //else
            //    DumbyIcon.transform.position = lastVaiablePosition;
        }

        bool CheckIfInsideTheScreen()
        {
            Vector3[] Corners = new Vector3[4];
            DumbyIcon.GetComponent<RectTransform>().GetWorldCorners(Corners);

            foreach (Vector3 corner in Corners)
            {
                if(!screenRect.Contains(corner))
                {
                    return false;
                }
            }
            return true;
        }


    }
}