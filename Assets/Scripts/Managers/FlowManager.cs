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
                case FlowState.MenuState:

                    break;
                case FlowState.GameplayState:
                    // Fa partire il gioco
                    GameManager.I.EnterGameplayMode();
                    
                    break;
                case FlowState.Pause:

                    break;
                case FlowState.ExitGameplay:

                    break;
                default:
                    break;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.I.UIMng.GamePlayCtrl.ActivatePause();
                CurrentState = FlowState.Pause;
            }
        }

    }

    public enum FlowState
    {
        MenuState,
        GameplayState,
        Pause,
        ExitGameplay,
    }
}