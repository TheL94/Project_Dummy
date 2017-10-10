using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using UnityEngine.UI;
using System.Linq;

namespace DumbProject.UI
{
    public class DumbyIndicatorMovement : MonoBehaviour
    {
        UIManager uiManager;
        public GameObject DumbyIconPrefab;
        GameObject DumbyIcon;
        Rect screenRect;
        Vector3 dumbyPosition;

        Vector3 lastVaiablePosition;

        //private Rect _screenRect;
        //public Rect ScreenRect
        //{
        //    get { return _screenRect; }
        //    set
        //    {
        //        /// Calcoli per determinare la dimensione della rect
        //        /// Di base restituisce lo screenRect, necessita delle ancore del camera panel per impostare le misure
        //        _screenRect = value;
        //    }
        //}


        private void Start()
        {
            uiManager = GameManager.I.UIMng;
            RectTransform cameraPanelRectTransform = uiManager.CameraPanel.GetComponent<RectTransform>();
            
            screenRect = new Rect(cameraPanelRectTransform.anchorMin.x * Screen.width, cameraPanelRectTransform.anchorMin.y * Screen.height, cameraPanelRectTransform.anchorMax.x * Screen.width, cameraPanelRectTransform.anchorMax.y * Screen.height);
            
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
            ScreenIndicatorStatus currentStatus = CheckIfInsideTheScreen();

            //lastVaiablePosition = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 100;
            if (GetComponentInParent<Renderer>().isVisible)
                currentStatus = 0;

            dumbyPosition = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 70;

            switch (currentStatus)
            {
                case ScreenIndicatorStatus.OnScreen:
                    DumbyIcon.transform.position = dumbyPosition;
                    break;
                case ScreenIndicatorStatus.XOutScreen:
                    DumbyIcon.transform.position = new Vector3(lastVaiablePosition.x, dumbyPosition.y);
                    break;
                case ScreenIndicatorStatus.YOutScreen:
                    DumbyIcon.transform.position = new Vector3(dumbyPosition.x, lastVaiablePosition.y);
                    break;
                case ScreenIndicatorStatus.BothOutScreen:
                    DumbyIcon.transform.position = lastVaiablePosition;
                    break;
            }
            if(currentStatus != ScreenIndicatorStatus.BothOutScreen)
                lastVaiablePosition = DumbyIcon.transform.position;
        }

        /// <summary>
        /// Controlla se l'indicatore di Dumby si trova all'interno dello schermo
        /// </summary>
        /// <returns>Ritorna quale asse si trova fuori dallo schermo</returns>
        ScreenIndicatorStatus CheckIfInsideTheScreen()
        {
            Vector3[] Corners = new Vector3[4];
            DumbyIcon.GetComponent<RectTransform>().GetWorldCorners(Corners);

            foreach (Vector3 corner in Corners)
            {
                if(!screenRect.Contains(corner))
                {
                    DumbyIcon.transform.position = lastVaiablePosition;
                    if (corner.x <= 0 && corner.y <= 0 || corner.x >= screenRect.width && corner.y <= 0 ||
                        corner.x <= 0 && corner.y >= screenRect.height || corner.x >= screenRect.width && corner.y >= screenRect.height)
                    {
                        if (dumbyPosition.x > 0 && dumbyPosition.x < screenRect.width)
                            return ScreenIndicatorStatus.YOutScreen;
                        
                        if (dumbyPosition.y < screenRect.height && dumbyPosition.y > 0)
                            return ScreenIndicatorStatus.XOutScreen;
                        
                        return ScreenIndicatorStatus.BothOutScreen;
                    }
                    if (corner.x <= 0 || corner.x >= screenRect.width)
                        return ScreenIndicatorStatus.XOutScreen;
                    
                    if (corner.y >= screenRect.height || corner.y <= 0)
                        return ScreenIndicatorStatus.YOutScreen;
                    
                }
            }
            
            return ScreenIndicatorStatus.OnScreen;
        }

        enum ScreenIndicatorStatus
        {
            OnScreen = 0,
            XOutScreen = 1,
            YOutScreen = 2,
            BothOutScreen = 3,
        }
    }
}