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

        public void Start()
        {
            canvasGame = Instantiate(Resources.Load("Prefabs/UI/CanvasGame") as GameObject, transform);

            MenuController = GetComponentInChildren<MenuPanelController>();
            GamePlayCtrl = GetComponentInChildren<UIGamePlayController>();
            MenuController.Init(this);
            GamePlayCtrl.Init(this);
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
            GameManager.I.flowMng.CurrentState = Flow.FlowState.GameplayState;
        }


    }
}