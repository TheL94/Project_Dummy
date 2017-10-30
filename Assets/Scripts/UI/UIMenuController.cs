using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DumbProject.Generic;
using System;

namespace DumbProject.UI
{
    public class UIMenuController : UIChanger
    {
        public MainMenuController MainMenuPanel;
        public CreditsMenuController CreditsPanel;

        UIManager uiManager;

        #region API
        public void Init(UIManager _uiManager)
        {
            uiManager = _uiManager;
            MainMenuPanel.Init(this);
            CreditsPanel.Init(this);
            ImageToChange = GetComponent<Image>();
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