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

            RoomPanelContainer.Setup(uiManager.DeviceCurrentOrientation);
            PausePanel.Init(this, uiManager.DeviceCurrentOrientation);
        }

        public void Setup()
        {
            ActivateLateralGUI(false);
            ActivatePausePanel(false);
            ActivatePauseButton(false);
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