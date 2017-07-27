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
        UIPositionData positionData;

        #region API
        public void Init(UIManager _uiManager, UIPositionData _positionData)
        {
            uiManager = _uiManager;
            positionData = _positionData;
            uiManager.SetRectTransformParametersByData(transform as RectTransform, _positionData);
        }

        #region MainMenuAPI
        public void PlayGame()
        {
            GameManager.I.ChageFlowState(Flow.FlowState.GameplayState);
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
    }
}