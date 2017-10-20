using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class UIMenuController : MonoBehaviour
    {
        public MainMenuController MainMenuPanel;
        public CreditsMenuController CreditsPanel;

        UIManager uiManager;

        #region API
        public void Init(UIManager _uiManager)
        {
            uiManager = _uiManager;
            MainMenuPanel.Init(this, uiManager.DeviceCurrentOrientation);
            CreditsPanel.Init(this, uiManager.DeviceCurrentOrientation);
        }

        public void Setup()
        {
            MainMenuPanel.gameObject.SetActive(true);
            CreditsPanel.gameObject.SetActive(false);
        }

        #region MainMenuAPI
        public void PlayGame()
        {
            GameManager.I.ChageFlowState(Flow.FlowState.EnterGameplay);
        }

        public void ShowCredits()
        {
            CreditsPanel.gameObject.SetActive(true);
            MainMenuPanel.gameObject.SetActive(false);
        }

        public void QuitApplication()
        {
            GameManager.I.ChageFlowState(Flow.FlowState.ExitGame);
        }
        #endregion

        #region CreditsMenuAPI
        public void ShowMainMenu()
        {
            MainMenuPanel.gameObject.SetActive(true);
            CreditsPanel.gameObject.SetActive(false);
        }
        #endregion
        #endregion
    }
}