using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using UnityEngine.UI;

namespace DumbProject.UI
{
    public class UIGamePlayController : MonoBehaviour
    {
        UIManager uiManager;

        public LateralGUIContainer LateralGUI;
        public PausePanelController PausePanel;

        UIPositionData positionData;

        #region API
        public void Init(UIManager _uiManager, UIPositionData _positionData)
        {
            uiManager = _uiManager;
            positionData = _positionData;

            uiManager.SetRectTransformParameters(transform as RectTransform, _positionData);

            LateralGUI.Init();
            LateralGUI.gameObject.SetActive(false);
            PausePanel.gameObject.SetActive(false);
        }

        #region PauseRegion
        /// <summary>
        /// Attiva il pannello della pausa
        /// </summary>
        public void ActivatePause(bool _status)
        {
            if(_status)
                GameManager.I.ChageFlowState(Flow.FlowState.Pause);
            else
                GameManager.I.ChageFlowState(Flow.FlowState.GameplayState);
        }

        /// <summary>
        /// Esce dallo stato di gameplay per andare nello stato di menu
        /// </summary>
        public void QuitGamePlay()
        {
            ActivatePause(false);
            GameManager.I.ChageFlowState(Flow.FlowState.ExitGameplay);
        }

        #endregion
        #endregion
    }
}