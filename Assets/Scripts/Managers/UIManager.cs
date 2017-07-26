using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using DumbProject.CameraController;

namespace DumbProject.UI
{
    public class UIManager : MonoBehaviour
    {
        public bool _isVertical;

        public bool IsVertical { get { return _isVertical; } set { _isVertical = value; } }

        [HideInInspector]
        public UIMenuController MenuController;
        [HideInInspector]
        public UIGameplayController GamePlayCtrl;
        [HideInInspector]
        public CameraInput CamInput;

        public void Init()
        {
            GameObject cameraInputObj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/CameraMovementPanel"), transform);
            SetupCameraByEnvironment(cameraInputObj);
            CameraHandler cc = Camera.main.GetComponent<CameraHandler>();
            CamInput.Init(this, cc);
            MenuController = Instantiate(Resources.Load<GameObject>("Prefabs/UI/MenuController"), transform).GetComponent<UIMenuController>();
            GamePlayCtrl = Instantiate(Resources.Load<GameObject>("Prefabs/UI/GameplayPanel"), transform).GetComponent<UIGameplayController>();
            MenuController.Init(this);
            GamePlayCtrl.Init(this);
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
                        AdaptTheUI(IsVertical);
                    }
                    else
                    {
                        IsVertical = true;
                        AdaptTheUI(IsVertical);
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
            if (Screen.orientation == ScreenOrientation.Portrait)
            {
                // La UI deve essere orientata per l'utilizzo verticale;
                IsVertical = true;
                if (MenuController.gameObject.activeInHierarchy)
                    MenuController.SetVerticalUI(true); 
                if (GamePlayCtrl.gameObject.activeInHierarchy)
                    GamePlayCtrl.SetVerticalGameUI(true);
            }
            else
            {
                // Gli altri orientamenti
                IsVertical = false;
                if (MenuController.gameObject.activeInHierarchy)
                    MenuController.SetVerticalUI(false);
                if (GamePlayCtrl.gameObject.activeInHierarchy)
                    GamePlayCtrl.SetVerticalGameUI(false);
            }
        }


        /// <summary>
        /// Usata solo per impostare manualmente la UI
        /// </summary>
        /// <param name="_isVertical"></param>
        void AdaptTheUI(bool _isVertical)
        {
            if (_isVertical)
            {
                // La UI deve essere orientata per l'utilizzo verticale;
                IsVertical = true;
                if (MenuController.gameObject.activeInHierarchy)
                    MenuController.SetVerticalUI(true);
                if (GamePlayCtrl.gameObject.activeInHierarchy)
                    GamePlayCtrl.SetVerticalGameUI(true);
            }
            else
            {
                // Gli altri orientamenti
                IsVertical = false;
                if (MenuController.gameObject.activeInHierarchy)
                    MenuController.SetVerticalUI(false);
                if (GamePlayCtrl.gameObject.activeInHierarchy)
                    GamePlayCtrl.SetVerticalGameUI(false);
            }
        }
    }
}