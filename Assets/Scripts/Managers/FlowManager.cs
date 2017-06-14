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
            set {
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
                    GameManager.I.UIMng.ActivateMenuPanel();
                    break;
                case FlowState.GameplayState:
                    // Fa partire il gioco
                    GameManager.I.UIMng.ActivateGamePlayPanel();
                    GameManager.I.EnterGameplayMode();
                    break;
                case FlowState.Pause:
                    // Blocca Dummy e attiva il menu di pausa
                    GameManager.I.ActivePauseMode();
                    break;
                case FlowState.ExitGameplay:
                    GameManager.I.ExitGameplayMode();
                    CurrentState = FlowState.MenuState;
                    break;
                default:
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

    }

    public enum FlowState
    {
        Loading,
        MenuState,
        GameplayState,
        Pause,
        ExitGameplay,
    }
}