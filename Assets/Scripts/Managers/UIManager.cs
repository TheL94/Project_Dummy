using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;
using DumbProject.CameraController;

namespace DumbProject.UI
{
    public class UIManager : MonoBehaviour
    {
        bool _isVertical;
        public bool IsVertical { get { return _isVertical; } set { _isVertical = value; } }

        [HideInInspector]
        public UIMenuController MenuController;
        [HideInInspector]
        public UIGamePlayController GamePlayCtrl;
        [HideInInspector]
        public CameraInput CamInput;

        public ScreenOrientation DeviceCurrentOrientation { get { return Screen.orientation; } }
        public Vector2 DeviceReferenceResolution { get { return new Vector2(Screen.currentResolution.width, Screen.currentResolution.height); } }

        string DataPath = "Data/UIData/";
        string PrefabPath = "Prefabs/UI/";

        public void Init()
        {
            //setta la risoluzione di riferimento del canvas in base a quella dello schermo del dispositivo in uso
            CanvasScaler scaler = GetComponent<CanvasScaler>();
            scaler.referenceResolution = DeviceReferenceResolution;
            // setup menu panel
            MenuController = Instantiate(Resources.Load<GameObject>(PrefabPath + "MenuController"), transform).GetComponent<UIMenuController>();
            MenuController.Init(this, Instantiate(Resources.Load<UIPositionData>(DataPath + "MenuPanelPosition")));
            // setup gameplay panel
            GamePlayCtrl = Instantiate(Resources.Load<GameObject>(PrefabPath + "GameplayPanel"), transform).GetComponent<UIGamePlayController>();
            GamePlayCtrl.Init(this, Instantiate(Resources.Load<UIPositionData>(DataPath + "GameplayPanelPosition")));
            // setup camera panel
            GameObject cameraInputObj = Instantiate(Resources.Load<GameObject>(PrefabPath + "CameraMovementPanel"), transform);
            SetupCameraByEnvironment(cameraInputObj);
            CameraHandler cameraHandler = Camera.main.GetComponent<CameraHandler>();
            CamInput.Init(this, cameraHandler, Instantiate(Resources.Load<UIPositionData>(DataPath + "CameraPanelPosition")));
        }

        public void SetRectTransformParametersByData(RectTransform _rcTransform, UIPositionData _data)
        {
            RectTranformValues values = null;
            if (GameManager.I.DeviceEnvironment == DeviceType.Desktop)
            {
                values = _data.orizontalRectValues;
            }
            else if (GameManager.I.DeviceEnvironment == DeviceType.Handheld)
            {
                if (DeviceCurrentOrientation == ScreenOrientation.Landscape)
                    values = _data.orizontalRectValues;
                else
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
                _rcTransform.offsetMax = _offsetMax;
        }

        public void ActivateMenuPanel(bool _status)
        {
            MenuController.gameObject.SetActive(_status);
        }

        public void ActivateGamePlayPanel(bool _status)
        {
            GamePlayCtrl.gameObject.SetActive(_status);
        }

        public void ActivateCameraPanel(bool _status)
        {
            CamInput.gameObject.SetActive(_status);
        }

        /// <summary>
        /// Controlla il dispositivo su cui è setta di conseguenza il tipo di input per la camera 
        /// </summary>
        void SetupCameraByEnvironment(GameObject _camInputObj)
        {
            if (GameManager.I.DeviceEnvironment == DeviceType.Desktop)
            {
                CamInput = _camInputObj.AddComponent<CameraMouseInput>();
            }
            else if (GameManager.I.DeviceEnvironment == DeviceType.Handheld)
            {
                CamInput = _camInputObj.AddComponent<CameraTouchInput>();
            }
            else
                Debug.LogWarning("Device Type Not Valid");
        }

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