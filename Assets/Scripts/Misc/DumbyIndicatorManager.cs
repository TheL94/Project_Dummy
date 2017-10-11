using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using UnityEngine.UI;
using System.Linq;

namespace DumbProject.UI
{
    public class DumbyIndicatorManager : MonoBehaviour
    {
        UIManager uiManager;
        public GameObject DumbyIconPrefab;
        GameObject DumbyIcon;
        Rect screenRect;
        Vector3 dumbyPosition;
        float offset;

        Vector3 lastVaiablePosition;

        bool XPos;
        bool XNeg;
        bool YPos;
        bool YNeg;


        private void Start()
        {
            offset = 5;
            uiManager = GameManager.I.UIMng;
            RectTransform cameraPanelRectTransform = uiManager.CameraPanel.GetComponent<RectTransform>();
            screenRect = new Rect(cameraPanelRectTransform.anchorMin.x * Screen.width, cameraPanelRectTransform.anchorMin.y * Screen.height, cameraPanelRectTransform.anchorMax.x * Screen.width, cameraPanelRectTransform.anchorMax.y * Screen.height);
            Vector3 vector = Camera.main.WorldToScreenPoint(transform.position) ;
            if (uiManager != null) {
                DumbyIcon = Instantiate(DumbyIconPrefab, vector + Vector3.up * 70, Quaternion.identity, uiManager.transform);
                DumbyIcon.GetComponent<DumbyIndicatorRepositioning>().Init(this);
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
            dumbyPosition = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 70;
            CheckIfInsideTheScreen();


            if (GetComponentInParent<Renderer>().isVisible)
            {
                DumbyIcon.transform.position = dumbyPosition;
                XPos=false;
                XNeg=false;
                YPos=false;
                YNeg = false;
            }

            if (XPos)
            {
                if (YPos)
                    DumbyIcon.transform.position = new Vector3(screenRect.width - offset, screenRect.height - offset, 0);
                else if (YNeg)
                    DumbyIcon.transform.position = new Vector3(screenRect.width - offset, screenRect.yMin + offset, 0);
                else
                    DumbyIcon.transform.position = new Vector3(screenRect.width - offset, dumbyPosition.y, 0);
            }
            if (XNeg)
            {
                if (YPos)
                    DumbyIcon.transform.position = new Vector3(0 + offset, screenRect.height - offset, 0);
                else if (YNeg)
                    DumbyIcon.transform.position = new Vector3(0 + offset, screenRect.yMin + offset, 0);
                else
                    DumbyIcon.transform.position = new Vector3(0 + offset, dumbyPosition.y, 0);
            }
            if (YPos)
            {
                if (XPos)
                    DumbyIcon.transform.position = new Vector3(screenRect.width - offset, screenRect.height - offset, 0);
                else if(XNeg)
                    DumbyIcon.transform.position = new Vector3(0 + offset, screenRect.height - offset, 0);
                else
                    DumbyIcon.transform.position = new Vector3(dumbyPosition.x, screenRect.height - offset, 0);
            }

            if (YNeg)
            {
                if (XPos)
                    DumbyIcon.transform.position = new Vector3(screenRect.width - offset, screenRect.yMin + offset, 0);
                else if (XNeg)
                    DumbyIcon.transform.position = new Vector3(0 + offset, screenRect.yMin + offset, 0);
                else
                    DumbyIcon.transform.position = new Vector3(dumbyPosition.x, screenRect.yMin + offset, 0);
            }

            lastVaiablePosition = DumbyIcon.transform.position;
        }

        /// <summary>
        /// Controlla se l'indicatore di Dumby si trova all'interno dello schermo
        /// </summary>
        /// <returns>Ritorna quale asse si trova fuori dallo schermo</returns>
        void CheckIfInsideTheScreen()
        {
            if (dumbyPosition.x < 0)
                XNeg = true;
            else
                XNeg = false;

            if (dumbyPosition.x > screenRect.width)
                XPos = true;
            else
                XPos = false;

            if (dumbyPosition.y < screenRect.yMin)
                YNeg = true;
            else
                YNeg = false;

            if (dumbyPosition.y > screenRect.height)
                YPos = true;
            else
                YPos = false;

        }

        
    }
}