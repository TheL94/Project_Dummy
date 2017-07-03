using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class UIManager : MonoBehaviour
    {
        GameObject canvasGame;

        public bool _isVertical;

        public bool IsVertical { get { return _isVertical; } set { _isVertical = value; } }

        [HideInInspector]
        public MenuPanelController MenuController;
        [HideInInspector]
        public UIGamePlayController GamePlayCtrl;

        public void Init()
        {
            canvasGame = Instantiate(Resources.Load("Prefabs/UI/CanvasGame") as GameObject, transform);
            MenuController = GetComponentInChildren<MenuPanelController>();
            GamePlayCtrl = GetComponentInChildren<UIGamePlayController>();
            CheckRunningMachine();
            MenuController.Init(this);
            GamePlayCtrl.Init(this);
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
        /// Controlla il dispositivo su cui è setta la l'orientamento di conseguenza
        /// </summary>
        void CheckRunningMachine()
        {
            switch (GameManager.I.DeviceEnviroment)
            {
                case DeviceType.Unknown:
                    break;
                case DeviceType.Handheld:
                    IsVertical = true;
                    break;
                case DeviceType.Console:
                    break;
                case DeviceType.Desktop:
                    IsVertical = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Attiva il pannello del gameplay, disattiva il pannello del menu
        /// </summary>
        public void ActivateGamePlayPanel()
        {
            GamePlayCtrl.Setup();
            MenuController.ChildPanel.gameObject.SetActive(false);
        }

        public void DestroyCanvasGame()
        {
            Destroy(canvasGame);
        }

        
        private void Update()
        {
            if(GameManager.I.DeviceEnviroment == DeviceType.Handheld)
                AdaptTheUI();

            if(GameManager.I.DeviceEnviroment == DeviceType.Desktop)
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