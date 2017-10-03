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

        public RoomPanelContainer RoomPanelContainer;
        public PausePanelController PausePanel;
        public Button PauseButton;

        #region API
        public void Init(UIManager _uiManager)
        {
            uiManager = _uiManager;

            RoomPanelContainer.Setup();
            PausePanel.Init(this);
            //InitPauseButton();
        }

        public void Setup()
        {
            ActivateLateralGUI(false);
            ActivatePausePanel(false);
            ActivatePauseButton(false);
        }

        void InitPauseButton()
        {
            float xMin = -3f;
            float yMin = 0.9f;

            float xMax;
            float yMax = 1f;

            if (uiManager.DeviceCurrentOrientation == ScreenOrientation.Landscape || GameManager.I.DeviceEnvironment == DeviceType.Desktop)
                 xMax = (xMin) + (yMax - yMin) * (uiManager.DeviceReferenceResolution.y / (uiManager.DeviceReferenceResolution.x * 0.25f));
            else
                 xMax = 1 / ((xMin) + (yMax - yMin) * (uiManager.DeviceReferenceResolution.y / (uiManager.DeviceReferenceResolution.x * 0.25f)));

            Vector2 anchorMin = new Vector2(xMin, yMin);
            Vector2 anchorMax = new Vector2(xMax, yMax);
            uiManager.SetRectTransformParametersByValues(PauseButton.transform as RectTransform, anchorMin, anchorMax, Vector2.zero, Vector2.zero);
        }

        /// <summary>
        /// Attiva il pannello della GUI laterale
        /// </summary>
        public void ActivateLateralGUI(bool _status)
        {
            RoomPanelContainer.gameObject.SetActive(_status);
        }

        /// <summary>
        /// Attiva il pannello della pausa
        /// </summary>
        public void ActivatePausePanel(bool _status)
        {
            PausePanel.gameObject.SetActive(_status);
            PauseButton.gameObject.SetActive(!_status);
        }

        /// <summary>
        /// Attiva il bottone della pausa
        /// </summary>
        public void ActivatePauseButton(bool _status)
        {
            PauseButton.gameObject.SetActive(_status);
        }
        #endregion

        private void OnEnable()
        {
            PauseButton.onClick.AddListener(() => { GameManager.I.ChageFlowState(Flow.FlowState.Pause); });
        }

        private void OnDisable()
        {           
            PauseButton.onClick.RemoveAllListeners();
        }
    }
}