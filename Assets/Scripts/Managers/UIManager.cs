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
        public UIGamePlayController UIGamePlayCtrl;

        public void Init()
        {
            canvasGame = Instantiate(Resources.Load("Prefabs/UI/CanvasGame") as GameObject, transform);
            MenuController = GetComponentInChildren<MenuPanelController>();
            UIGamePlayCtrl = GetComponentInChildren<UIGamePlayController>();
            MenuController.Init(this);
            UIGamePlayCtrl.Init(this);
        }

        /// <summary>
        /// Attiva il pannello del menu, disattiva il pannello del gameplay
        /// </summary>
        public void ActivateMenuPanel()
        {
            MenuController.gameObject.SetActive(true);
            UIGamePlayCtrl.gameObject.SetActive(false);
        }

        /// <summary>
        /// Attiva il pannello del gameplay, disattiva il pannello del menu
        /// </summary>
        public void ActivateGamePlayPanel()
        {
            UIGamePlayCtrl.gameObject.SetActive(true);
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
            UIGamePlayCtrl.gameObject.SetActive(true);
            GameManager.I.flowMng.CurrentState = Flow.FlowState.GameplayState;
        }


    }
}