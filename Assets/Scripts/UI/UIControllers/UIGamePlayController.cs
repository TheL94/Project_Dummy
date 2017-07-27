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
        public Button PauseButton;

        UIPositionData positionData;

        #region API
        public void Init(UIManager _uiManager, UIPositionData _positionData)
        {
            uiManager = _uiManager;
            positionData = _positionData;

            uiManager.SetRectTransformParametersByData(transform as RectTransform, _positionData);

            LateralGUI.Init();
            ActivateLateralGUI(false);
            ActivatePausePanel(false);
            InitPauseButton();
        }

        void InitPauseButton()
        {
            PauseButton.onClick.AddListener(() => { ActivatePausePanel(true); });
            float xMax = 1f + ((-3f) - 0.9f) / (uiManager.DeviceReferenceResolution.y / uiManager.DeviceReferenceResolution.x);
            Vector2 anchorMin = new Vector2(-3f, 0.9f);
            Vector2 anchorMax = new Vector2(xMax, 1f);
            uiManager.SetRectTransformParametersByValues(PauseButton.transform as RectTransform, anchorMin, anchorMax, Vector2.zero, Vector2.zero);
        }

        #region PauseRegion
        /// <summary>
        /// Attiva il pannello della GUI laterale
        /// </summary>
        public void ActivateLateralGUI(bool _status)
        {
            LateralGUI.gameObject.SetActive(_status);
        }

        /// <summary>
        /// Attiva il pannello della pausa
        /// </summary>
        public void ActivatePausePanel(bool _status)
        {
            PausePanel.gameObject.SetActive(_status);
        }

        /// <summary>
        /// Esce dallo stato di gameplay per andare nello stato di menu
        /// </summary>
        public void QuitGamePlay()
        {
            ActivatePausePanel(false);
            GameManager.I.ChageFlowState(Flow.FlowState.ExitGameplay);
        }

        #endregion
        #endregion
    }
}