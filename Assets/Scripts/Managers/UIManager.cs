using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class UIManager : MonoBehaviour
    {
        public bool ForceVerticalUI;

        [HideInInspector]
        public UIMenuController MenuController;
        [HideInInspector]
        public UIGamePlayController GamePlayCtrl;
        [HideInInspector]
        public UIEndGameplayController EndGameCtrl;
        [HideInInspector]
        public UICameraPanelController CameraPanel;

        ScreenOrientation deviceOrientation = ScreenOrientation.Unknown;
        public ScreenOrientation DeviceCurrentOrientation { get { return Screen.orientation; } }

        public Vector2 DeviceReferenceResolution
        {
            get
            {
                if (GameManager.I.DeviceEnvironment == DeviceType.Desktop)
                {
                    if (ForceVerticalUI)
                        return new Vector2(Screen.currentResolution.height, Screen.currentResolution.width);
                    else
                        return new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
                }
                else
                {
                    if (DeviceCurrentOrientation == ScreenOrientation.Portrait || DeviceCurrentOrientation == ScreenOrientation.PortraitUpsideDown)
                        return new Vector2(Screen.currentResolution.height, Screen.currentResolution.width);
                    else
                        return new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
                }
            }
        }
        public Vector2 CurrentResolution
        {
            get
            {
                if (GameManager.I.DeviceEnvironment == DeviceType.Desktop)
                {
                    if(ForceVerticalUI)
                        return new Vector2(Screen.height, Screen.width);
                    else
                        return new Vector2(Screen.width, Screen.height);
                }
                else
                {
                    if (DeviceCurrentOrientation == ScreenOrientation.Portrait || DeviceCurrentOrientation == ScreenOrientation.PortraitUpsideDown)
                        return new Vector2(Screen.height, Screen.width);
                    else
                        return new Vector2(Screen.width, Screen.height);
                }
            }
        }

        const string PrefabPath = "Prefabs/UI/";

        private void LateUpdate()
        {
            UpdateUIOrientation();
        }

        /// <summary>
        /// Funzione che scatena l'evento che fa aggiornare la ui se cambia l'orientamento del dispositivo
        /// </summary>
        void UpdateUIOrientation()
        {
            if (deviceOrientation != DeviceCurrentOrientation)
            {
                if (GameManager.I.DeviceEnvironment == DeviceType.Desktop)
                {
                    if (ForceVerticalUI)
                        deviceOrientation = ScreenOrientation.Portrait;
                    else
                        deviceOrientation = ScreenOrientation.Landscape;
                }
                else if (SystemInfo.deviceType == DeviceType.Handheld)
                {
                    deviceOrientation = DeviceCurrentOrientation;
                }

                OnScreenOrientationChange();
            }
        }

        public void Init()
        {
            //setta la risoluzione di riferimento del canvas in base a quella dello schermo del dispositivo in uso
            CanvasScaler scaler = GetComponent<CanvasScaler>();
            scaler.referenceResolution = DeviceReferenceResolution;
            // setup camera panel
            CameraPanel = Instantiate(Resources.Load<GameObject>(PrefabPath + "CameraMovementPanel"), transform).GetComponent<UICameraPanelController>();
            // setup menu panel
            MenuController = Instantiate(Resources.Load<GameObject>(PrefabPath + "MenuController"), transform).GetComponent<UIMenuController>();
            MenuController.Init(this);
            // setup gameplay panel
            GamePlayCtrl = Instantiate(Resources.Load<GameObject>(PrefabPath + "GameplayPanel"), transform).GetComponent<UIGamePlayController>();
            GamePlayCtrl.Init(this);
            // endgameplay panel
            EndGameCtrl = Instantiate(Resources.Load<GameObject>(PrefabPath + "EndGamePanel"), transform).GetComponent<UIEndGameplayController>();
            EndGameCtrl.Init(this);

            UpdateUIOrientation();

            MenuController.Setup();
            GamePlayCtrl.Setup();
            EndGameCtrl.Setup();
        }

        public void SetRectTransformParametersByData(RectTransform _rcTransform, UIPositionData _data)
        {
            RectTranformValues values = null;
            if (GameManager.I.DeviceEnvironment == DeviceType.Desktop)
            {
                if (ForceVerticalUI)
                    values = _data.verticalRectValues;
                else
                    values = _data.orizontalRectValues;
            }
            else if (GameManager.I.DeviceEnvironment == DeviceType.Handheld)
            {
                if (DeviceCurrentOrientation == ScreenOrientation.Landscape || DeviceCurrentOrientation == ScreenOrientation.LandscapeLeft || DeviceCurrentOrientation == ScreenOrientation.LandscapeRight)
                    values = _data.orizontalRectValues;
                else if(DeviceCurrentOrientation == ScreenOrientation.Portrait || DeviceCurrentOrientation == ScreenOrientation.PortraitUpsideDown)
                    values = _data.verticalRectValues;
            }

            if (values != null)
                SetRectTransformParametersByValues(_rcTransform, values.AnchorMin, values.AnchorMax, values.OffsetMin, values.OffsetMax);
            else
                Debug.LogWarning("Device Type Not Valid");
        }

        public void SetRectTransformParametersByValues(RectTransform _rcTransform, Vector2 _anchorMin, Vector2 _anchorMax, Vector2 _offsetMin, Vector2 _offsetMax)
        {
            _rcTransform.anchorMin = _anchorMin;
            _rcTransform.anchorMax = _anchorMax;
            _rcTransform.offsetMin = _offsetMin;
            _rcTransform.offsetMax = new Vector2(- _offsetMax.x, - _offsetMax.y);
        }

        public void ActivateMenuPanel(bool _status)
        {
            MenuController.gameObject.SetActive(_status);
        }

        public void ActivateGamePlayPanel(bool _status)
        {
            GamePlayCtrl.gameObject.SetActive(_status);
        }

        public void ActivateEndGameplayPanel(bool _status)
        {
            EndGameCtrl.gameObject.SetActive(_status);
        }

        public void ActivateCameraPanel(bool _status)
        {
            CameraPanel.gameObject.SetActive(_status);
        }

        /// <summary>
        /// Delegato che gestisce gli eventi relativi al layout
        /// </summary>
        public delegate void LayoutEvent();
        /// <summary>
        /// Evento che viene scatenato al cambio di orientamento del dispositivo
        /// </summary>
        public LayoutEvent OnScreenOrientationChange;
    }

    [System.Serializable]
    public class RectTranformValues
    {
        public Vector2 AnchorMin;
        public Vector2 AnchorMax;
        public Vector2 OffsetMin;
        public Vector2 OffsetMax;

        public RectTranformValues(Vector2 _anchorMin, Vector2 _anchorMax, Vector2 _offsetMin, Vector2 _offsetMax)
        {
            AnchorMin = _anchorMin;
            AnchorMax = _anchorMax;
            OffsetMin = _offsetMin;
            OffsetMax = _offsetMax;
        }
    }
}