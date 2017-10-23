using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using UnityEngine.UI;
using FlexibleUI;

namespace DumbProject.UI
{
    public class DumbyIndicatorManager : MonoBehaviour
    {
        UIManager uiManager;

        private ScreenOrientation _tempDeviceOrientation;
        private ScreenOrientation TempDeviceOrientation
        {
            get
            {
                return _tempDeviceOrientation;
            }
            set
            {
                if (_tempDeviceOrientation != value)
                {
                    _tempDeviceOrientation = value;
                }
            }
        }

        ScreenOrientation DeviceOrientation
        {
            get
            {
                return uiManager.DeviceCurrentOrientation;
            }
        }

        public Vector2 UpLeftFrameCorner;

        public GameObject DumbyIconPrefab;
        GameObject DumbyIcon;
        Vector3 dumbyPosition;
        float offset;

        public void Init()
        {
            offset = 5;
            uiManager = GameManager.I.UIMng;
            Vector3 vector = Camera.main.WorldToScreenPoint(transform.position);
            if (uiManager != null)
            {
                DumbyIcon = Instantiate(DumbyIconPrefab, vector + Vector3.up * 70, Quaternion.identity, uiManager.transform);
                DumbyIcon.GetComponent<DumbyIndicatorRepositioning>().Init(this);
            }
        }

        private void Update()
        {
            if (uiManager != null)
            {
                _tempDeviceOrientation = DeviceOrientation;
            }

            if (GameManager.I.CurrentState == Flow.FlowState.Pause)
                DumbyIcon.GetComponent<Image>().enabled = false;
            else if (GameManager.I.CurrentState == Flow.FlowState.Gameplay)
            {
                DumbyIcon.GetComponent<Image>().enabled = true;
                UpdateIndicatorPosition();
            }
            else if (GameManager.I.CurrentState == Flow.FlowState.RecapGame)
                Destroy(DumbyIcon);
        }

        /// <summary>
        /// Setta la posizione dell'indicare in UI
        /// </summary>
        void UpdateIndicatorPosition()
        {
            dumbyPosition = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 70;
            Vector3 iconPos = dumbyPosition;
            Vector3 iconNewPos = iconPos;

            if (CheckIfInsideTheScreen(dumbyPosition))
            {
                iconNewPos = dumbyPosition;
            }
            else
            {
                if (iconPos.x >= Screen.width - offset)
                    if (iconPos.y < UpLeftFrameCorner.y + offset + float.Epsilon)
                        iconNewPos = new Vector3(Screen.width - offset, UpLeftFrameCorner.y, 0);
                    else if (iconPos.y >= Screen.height - offset)
                        iconNewPos = new Vector3(Screen.width - offset, Screen.height - offset, 0);
                    else
                        iconNewPos = new Vector3(Screen.width - offset, dumbyPosition.y, 0);

                if (iconPos.x > UpLeftFrameCorner.x - offset && iconPos.x < Screen.width - offset)
                    if (iconPos.y <= UpLeftFrameCorner.y + offset + float.Epsilon)
                        iconNewPos = new Vector3(dumbyPosition.x, UpLeftFrameCorner.y - offset, 0);
                    else if (iconPos.y >= Screen.height - offset)
                        iconNewPos = new Vector3(dumbyPosition.x, Screen.height - offset, 0);
                    else
                        iconNewPos = new Vector3(dumbyPosition.x, dumbyPosition.y, 0);

                if (iconPos.x <= UpLeftFrameCorner.x - offset && iconPos.x >= offset)
                    if (iconPos.y <= offset + float.Epsilon)
                        iconNewPos = new Vector3(dumbyPosition.x, offset, 0);
                    else if (iconPos.y >= Screen.height - offset)
                        iconNewPos = new Vector3(dumbyPosition.x, Screen.height - offset, 0);
                    else
                        iconNewPos = new Vector3(dumbyPosition.x, dumbyPosition.y, 0);

                if (iconPos.x < offset)
                    if (iconPos.y <= offset + float.Epsilon)
                        iconNewPos = new Vector3(offset, offset, 0);
                    else if (iconPos.y >= Screen.height - offset)
                        iconNewPos = new Vector3(offset, Screen.height - offset, 0);
                    else
                        iconNewPos = new Vector3(offset, dumbyPosition.y, 0);
            }

            DumbyIcon.transform.position = iconNewPos;
        }

        /// <summary>
        /// Controlla se l'indicatore di Dumby si trova all'interno dello schermo
        /// </summary>
        /// <returns>Ritorna quale asse si trova fuori dallo schermo</returns>
        bool CheckIfInsideTheScreen(Vector2 pos)
        {
            if (pos.x > 0 && pos.y > 0 && pos.x < Screen.width && pos.y < Screen.height)
                if (!(pos.x > UpLeftFrameCorner.x && pos.y < UpLeftFrameCorner.y))
                    return true;

            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            //Draw della orizzontale
            Gizmos.DrawLine(UpLeftFrameCorner, new Vector2(UpLeftFrameCorner.x, Screen.width));
            //Draw della Verticale
            Gizmos.DrawLine(UpLeftFrameCorner, new Vector2(UpLeftFrameCorner.x, 0));
        }
    }
}