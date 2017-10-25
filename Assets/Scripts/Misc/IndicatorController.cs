using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using UnityEngine.UI;


namespace DumbProject.UI
{
    public class IndicatorController : MonoBehaviour, IUIChanger
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
        public bool DrawGizmos;

        public Vector2 CurrentUiFrame;

        bool UiIsVertical;

        Vector2 verticalUiFrame = new Vector2(0, 0.175f);
        Vector2 horizontalUiFrame = new Vector2(0.43f , 0.314f);
        float offset = 5;

        GameObject IconPrefab;

        [HideInInspector]
        public GameObject Icon;

        Vector3 dumbyPosition;

        public void Init()
        {
            uiManager = GameManager.I.UIMng;
            Vector3 vector = Camera.main.WorldToScreenPoint(transform.position);
            if (uiManager != null)
            {
                IconPrefab = Resources.Load("Prefabs/Misc/IndicatorPrefab") as GameObject;
                Icon = Instantiate(IconPrefab, vector + Vector3.up * 70, Quaternion.identity, uiManager.transform);
            }
            Icon.AddComponent<IndicatorRepositioner>().Init(this);
            SetUIOrientation(GameManager.I.UIMng.DeviceCurrentOrientation);


        }

        private void Update()
        {
            if (uiManager != null)
            {
                _tempDeviceOrientation = DeviceOrientation;
            }

            if (GameManager.I.CurrentState == Flow.FlowState.Pause)
                Icon.GetComponent<Image>().enabled = false;
            else if (GameManager.I.CurrentState == Flow.FlowState.Gameplay)
            {
                Icon.GetComponent<Image>().enabled = true;
                UpdateIndicatorPosition();
            }
            else if (GameManager.I.CurrentState == Flow.FlowState.RecapGame)
                Destroy(Icon);
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
                    if (iconPos.y < CurrentUiFrame.y + offset + float.Epsilon)
                        iconNewPos = new Vector3(Screen.width - offset, CurrentUiFrame.y, 0);
                    else if (iconPos.y >= Screen.height - offset)
                        iconNewPos = new Vector3(Screen.width - offset, Screen.height - offset, 0);
                    else
                        iconNewPos = new Vector3(Screen.width - offset, dumbyPosition.y, 0);

                if (iconPos.x > CurrentUiFrame.x - offset && iconPos.x < Screen.width - offset)
                    if (iconPos.y <= CurrentUiFrame.y + offset + float.Epsilon)
                        iconNewPos = new Vector3(dumbyPosition.x, CurrentUiFrame.y - offset, 0);
                    else if (iconPos.y >= Screen.height - offset)
                        iconNewPos = new Vector3(dumbyPosition.x, Screen.height - offset, 0);
                    else
                        iconNewPos = new Vector3(dumbyPosition.x, dumbyPosition.y, 0);

                if (iconPos.x <= CurrentUiFrame.x - offset && iconPos.x >= offset)
                    if (iconPos.y <= offset + float.Epsilon)
                        iconNewPos = new Vector3(dumbyPosition.x, offset, 0);
                    else if (iconPos.y >= Screen.height - offset)
                        iconNewPos = new Vector3(dumbyPosition.x, Screen.height - offset, 0);
                    else
                        iconNewPos = new Vector3(dumbyPosition.x, dumbyPosition.y, 0);

                if (iconPos.x < offset)
                {
                    if (!UiIsVertical)
                    {
                        if (iconPos.y <= offset + float.Epsilon)
                            iconNewPos = new Vector3(offset, offset, 0);
                        else if (iconPos.y >= Screen.height - offset)
                            iconNewPos = new Vector3(offset, Screen.height - offset, 0);
                        else
                            iconNewPos = new Vector3(offset, dumbyPosition.y, 0); 
                    }
                    else
                    {
                        if (iconPos.y <= CurrentUiFrame.y + float.Epsilon)
                            iconNewPos = new Vector3(offset, CurrentUiFrame.y, 0);
                        else if (iconPos.y >= Screen.height - offset)
                            iconNewPos = new Vector3(offset, Screen.height - offset, 0);
                        else
                            iconNewPos = new Vector3(offset, dumbyPosition.y, 0);
                    }
                }
            }

            Icon.transform.position = iconNewPos;
        }

        /// <summary>
        /// Controlla se l'indicatore di Dumby si trova all'interno dello schermo
        /// </summary>
        /// <returns>Ritorna quale asse si trova fuori dallo schermo</returns>
        bool CheckIfInsideTheScreen(Vector2 pos)
        {
            if (pos.x > 0 && pos.y > 0 && pos.x < Screen.width && pos.y < Screen.height)
                if (!(pos.x > CurrentUiFrame.x && pos.y < CurrentUiFrame.y))
                    return true;

            return false;
        }


        private void OnDrawGizmos()
        {
            if (DrawGizmos)
            {
                Gizmos.color = Color.magenta;

                Gizmos.DrawLine(CurrentUiFrame, new Vector3(Screen.currentResolution.width, CurrentUiFrame.y, 0));
                Gizmos.DrawLine(CurrentUiFrame, new Vector3(CurrentUiFrame.x, 0, 0));
            }
        }

        public void SetUIOrientation(ScreenOrientation _orientation)
        {
            if (GameManager.I.DeviceEnvironment == DeviceType.Desktop)
            {
                if (GameManager.I.UIMng.ForceVerticalUI)
                {
                    CurrentUiFrame = new Vector2(Screen.width * verticalUiFrame.x, Screen.height * verticalUiFrame.y);
                    UiIsVertical = true;
                }
                else
                {
                    CurrentUiFrame = new Vector2(Screen.width * horizontalUiFrame.x, Screen.height * horizontalUiFrame.y);
                    UiIsVertical = false;
                }
            }
            else
            {
                if (_orientation == ScreenOrientation.Portrait || _orientation == ScreenOrientation.PortraitUpsideDown)
                {
                    CurrentUiFrame = new Vector2(Screen.width * verticalUiFrame.x, Screen.height * verticalUiFrame.y);
                    UiIsVertical = true;
                }

                else
                {
                    CurrentUiFrame = new Vector2(Screen.width * horizontalUiFrame.x, Screen.height * horizontalUiFrame.y);
                    UiIsVertical = false;
                }

            }
        }
    }
}