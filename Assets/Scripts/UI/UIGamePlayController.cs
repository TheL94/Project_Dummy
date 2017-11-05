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
        public Button_PlayStop PlayStopButton;

        #region API
        public void Init(UIManager _uiManager)
        {
            uiManager = _uiManager;

            RoomPanelContainer.Setup();
            PausePanel.Init(this);
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

        /// <summary>
        /// Inizializza il bottone per cambiare lo stato della AI
        /// </summary>
        public void InitializePlayStopButton()
        {
            if (PlayStopButton.enabled == false)
                PlayStopButton.enabled = true;
            PlayStopButton.Init();
        }
        /// <summary>
        /// Active if the game is in gameplay mode (so the timer can rum)
        /// Inactive when the game is in Pause mode (the timer is stoped)
        /// </summary>
        public void SetNextTurnButtonStatus(bool _status)
        {
            PlayStopButton.enabled = !_status;
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