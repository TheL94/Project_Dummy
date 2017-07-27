using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;


namespace DumbProject.Flow
{
    public class FlowManager : MonoBehaviour
    {

        private FlowState _currentState;

        public FlowState CurrentState
        {
            get { return _currentState; }
            private set {
                _currentState = value;
                Debug.Log(_currentState.ToString());
                OnStateChange();
            }
        }
        
        private void OnStateChange()
        {
            switch (CurrentState)
            {
                case FlowState.Loading:
                    GameManager.I.Init();
                    break;
                case FlowState.MenuState:
                    GameManager.I.UIMng.ActivateMenuPanel(true);
                    GameManager.I.UIMng.ActivateGamePlayPanel(false);
                    GameManager.I.UIMng.ActivateCameraPanel(false);
                    break;
                case FlowState.GameplayState:
                    // Fa partire il gioco
                    GameManager.I.UIMng.ActivateGamePlayPanel(true);
                    GameManager.I.UIMng.ActivateMenuPanel(false);
                    GameManager.I.UIMng.ActivateCameraPanel(true);
                    GameManager.I.UIMng.GamePlayCtrl.ActivateLateralGUI(true);
                    GameManager.I.EnterGameplayMode();
                    break;
                case FlowState.Pause:
                    GameManager.I.UIMng.GamePlayCtrl.ActivatePausePanel(true);
                    break;
                case FlowState.ExitGameplay:
                    GameManager.I.ExitGameplayMode();
                    GameManager.I.UIMng.GamePlayCtrl.ActivateLateralGUI(false);
                    break;
                case FlowState.ExitGame:
                    Application.Quit();
                    break;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CurrentState = FlowState.Pause;
            }
        }

        public void ChageState(FlowState _stateToSet)
        {
            CurrentState = _stateToSet;
        }
    }

    public enum FlowState
    {
        Loading,
        MenuState,
        GameplayState,
        Pause,
        ExitGameplay,
        ExitGame
    }
}