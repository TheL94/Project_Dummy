﻿using System.Collections;
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

        #region API
        public void Init(UIManager _uiManager)
        {
            uiManager = _uiManager;

            LateralGUI.Init(this);
            PausePanel.Init(this);
            ActivateLateralGUI(false);
            ActivatePausePanel(false);
            InitPauseButton();
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

        private void OnEnable()
        {
            PauseButton.onClick.AddListener(() => { GameManager.I.ChageFlowState(Flow.FlowState.Pause); });
        }

        private void OnDisable()
        {           
            PauseButton.onClick.RemoveAllListeners();
        }

        #endregion
        #endregion
    }
}