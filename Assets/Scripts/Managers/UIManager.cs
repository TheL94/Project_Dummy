using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class UIManager : MonoBehaviour
    {
        GameObject canvasGame;

        [HideInInspector]
        public MenuPanelController MenuController;
        [HideInInspector]
        public UIGamePlayController GamePlayCtrl;

        public void Init()
        {
            canvasGame = Instantiate(Resources.Load("Prefabs/UI/CanvasGame") as GameObject, transform);
            MenuController = GetComponentInChildren<MenuPanelController>();
            GamePlayCtrl = GetComponentInChildren<UIGamePlayController>();
            MenuController.Init(this);
            GamePlayCtrl.Init(this);
        }

        /// <summary>
        /// Attiva il pannello del menu, disattiva il pannello del gameplay
        /// </summary>
        public void ActivateMenuPanel()
        {
            MenuController.gameObject.SetActive(true);
            GamePlayCtrl.gameObject.SetActive(false);
        }

        /// <summary>
        /// Attiva il pannello del gameplay, disattiva il pannello del menu
        /// </summary>
        public void ActivateGamePlayPanel()
        {
            GamePlayCtrl.gameObject.SetActive(true);
            MenuController.gameObject.SetActive(false);
        }

        public void DestroyCanvasGame()
        {
            Destroy(canvasGame);
        }

        /// <summary>
        /// Setta lo stato di gameplay nel FlowManager
        /// </summary>
        public void GoInGameplayMode()
        {
            GamePlayCtrl.gameObject.SetActive(true);
            GameManager.I.FlowMng.CurrentState = Flow.FlowState.GameplayState;
        }


    }
}