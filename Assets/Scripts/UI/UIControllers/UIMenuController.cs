using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class UIMenuController : MonoBehaviour
    {
        public GameObject MainMenuPanel;
        public GameObject CreditsPanel;

        UIManager uiManager;

        #region API
        public void Init(UIManager _uiManager)
        {
            uiManager = _uiManager;
        }

        #region MainMenuAPI
        public void PlayGame()
        {
            GameManager.I.ChageFlowState(Flow.FlowState.EnterGameplay);
        }

        public void ShowCredits()
        {
            CreditsPanel.SetActive(true);
            MainMenuPanel.SetActive(false);
        }

        public void QuitApplication()
        {
            GameManager.I.ChageFlowState(Flow.FlowState.ExitGame);
        }


        #endregion
        #endregion

        void SetupMainMenuPanel(UIPositionData _positionData)
        {
            uiManager.SetRectTransformParametersByData(MainMenuPanel.transform as RectTransform, _positionData);
        }
    }
}