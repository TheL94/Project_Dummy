﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Flow
{
    public class FlowManager : MonoBehaviour
    {
        private FlowState _currentState = FlowState.None;
        public FlowState CurrentState
        {
            get { return _currentState; }
            private set
            {
                if(_currentState != value)
                {
                    FlowState oldState = _currentState;
                    _currentState = value;
                    Debug.Log(_currentState.ToString());
                    OnStateChange(_currentState ,oldState);
                }
            }
        }
        
        private void OnStateChange(FlowState _newState, FlowState _oldState)
        {
            switch (_newState)
            {
                case FlowState.Loading:
                    GameManager.I.LoadingActions();
                    break;
                case FlowState.Menu:
                    GameManager.I.MenuActions();
                    break;
                case FlowState.EnterGameplay:
                    GameManager.I.EnterGameplayActions();
                    break;
                case FlowState.Gameplay:
                    if(_oldState == FlowState.Pause)
                        GameManager.I.PauseActions(false);
                    break;
                case FlowState.Pause:
                    GameManager.I.PauseActions(true);
                    break;
                case FlowState.ExitGameplay:
                    GameManager.I.ExitGameplayActions();
                    break;
                case FlowState.EndGame:
                    GameManager.I.EndGameActions();
                    break;
                case FlowState.ExitGame:
                    GameManager.I.QuitGameActions();
                    break;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ChageState(FlowState.Pause);
            }
        }

        public void ChageState(FlowState _stateToSet)
        {
            CurrentState = _stateToSet;
        }
    }

    public enum FlowState
    {
        None,
        Loading,
        Menu,
        EnterGameplay,
        Gameplay,
        Pause,
        ExitGameplay,
        EndGame,
        ExitGame
    }
}