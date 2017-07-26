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
        public UIGameplayController GamePlayCtrl;
        [HideInInspector]
        public CameraInput CamInput;

        public void Init()
        {
            //setta la risoluzione di riferimento del canvas in base a quella dello schermo del dispositivo in uso
            CanvasScaler scaler = GetComponent<CanvasScaler>();
            scaler.referenceResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
            // setup menu panel
            MenuController = Instantiate(Resources.Load<GameObject>("Prefabs/UI/MenuController"), transform).GetComponent<UIMenuController>();
            MenuController.Init(this);
            // setup gameplay panel
            GamePlayCtrl = Instantiate(Resources.Load<GameObject>("Prefabs/UI/GameplayPanel"), transform).GetComponent<UIGameplayController>();
            //GamePlayCtrl.Init(this);
            // setup camera panel
            GameObject cameraInputObj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/CameraMovementPanel"), transform);
            SetupCameraByEnvironment(cameraInputObj);
            CameraHandler cameraHandler = Camera.main.GetComponent<CameraHandler>();
            CamInput.Init(this, cameraHandler);
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
        }

        /// <summary>
        /// Attiva il pannello del menu, disattiva il pannello del gameplay
        /// </summary>
        public void ActivateMenuPanel()
        {
            MenuController.Setup();
            GamePlayCtrl.ChildPanel.gameObject.SetActive(false);
        }

        /// <summary>
        /// Attiva il pannello del gameplay, disattiva il pannello del menu
        /// </summary>
        public void ActivateGamePlayPanel()
        {
            GamePlayCtrl.Setup();
            MenuController.ChildPanel.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            if(GameManager.I.DeviceEnvironment == DeviceType.Handheld)
                AdaptTheUI();

            if(GameManager.I.DeviceEnvironment == DeviceType.Desktop)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    if (IsVertical)
                    {
                        IsVertical = false;
                        AdaptTheUI();
                    }
                    else
                    {
                        IsVertical = true;
                        AdaptTheUI();
                    }
                }
            }
        }


        /// <summary>
        /// Setta lo stato di gameplay nel FlowManager
        /// </summary>
        public void GoInGameplayMode()
        {
            GamePlayCtrl.gameObject.SetActive(true);
            GameManager.I.FlowMng.CurrentState = Flow.FlowState.GameplayState;
        }

        void AdaptTheUI()
        {
            
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