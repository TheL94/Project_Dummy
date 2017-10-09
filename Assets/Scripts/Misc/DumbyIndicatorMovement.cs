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

        private Vector3 lastVaiablePosition;

        //public Vector3 LastVaiablePosition
        //{
        //    get { return lastVaiablePosition; }
        //    set { lastVaiablePosition = value;
        //        CheckIfInsideTheScreen();
        //    }
        //}


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
            ScreenIndicatorStatus currentStatus = CheckIfInsideTheScreen();

            //lastVaiablePosition = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 100;
            if (GetComponentInParent<Renderer>().isVisible)
                currentStatus = 0;

            Vector3 dumbyPosition;
            switch (currentStatus)
            {
                case ScreenIndicatorStatus.OnScreen:
                    DumbyIcon.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 70;
                    lastVaiablePosition = DumbyIcon.transform.position;
                    break;
                case ScreenIndicatorStatus.XOutScreen:
                    dumbyPosition = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 70;
                    DumbyIcon.transform.position = new Vector3(lastVaiablePosition.x, dumbyPosition.y);
                    lastVaiablePosition = DumbyIcon.transform.position;
                    break;
                case ScreenIndicatorStatus.YOutScreen:
                    dumbyPosition = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 70;
                    DumbyIcon.transform.position = new Vector3(dumbyPosition.x, lastVaiablePosition.y);
                    lastVaiablePosition = DumbyIcon.transform.position;
                    break;
                case ScreenIndicatorStatus.BothOutScreen:
                    DumbyIcon.transform.position = lastVaiablePosition;
                    break;
            }
            //Debug.Log(lastVaiablePosition);
            Debug.Log(currentStatus);
        }

        ScreenIndicatorStatus CheckIfInsideTheScreen()
        {
            Vector3[] Corners = new Vector3[4];
            DumbyIcon.GetComponent<RectTransform>().GetWorldCorners(Corners);

            foreach (Vector3 corner in Corners)
            {
                if(!screenRect.Contains(corner))
                {
                    DumbyIcon.transform.position = lastVaiablePosition;
                    //if (/*corner.x <= 0 && corner.y <= 0 || corner.x >= screenRect.width && corner.y <= 0 || */
                    //    corner.x <= 0 && corner.y >= screenRect.height /*|| corner.x >= screenRect.width && corner.y >= screenRect.height*/)
                    //{
                    //    return ScreenIndicatorStatus.BothOutScreen;
                    //}
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