using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Test.AI
{
    public class AI_Controller : MonoBehaviour
    {
        public bool IsActive = true;
        private AI_State _currentState;
        public AI_State CurrentState
        {
            get { return _currentState; }
            set { OnCurrentStateChange(_currentState, value); }
        }

        public void Init(AI_State currentState)
        {
            CurrentState = currentState;
        }

        private void Update()
        {
            if (IsActive)
            {
                CurrentState.ExecuteLoopActions(this);
            }
        }

        private void OnCurrentStateChange(AI_State oldState, AI_State newState)
        {
            oldState.Clean();

            AI_State newStateInstance = AI_DataManager.GetState(this, newState);
            Debug.Log(newStateInstance.name + " instance: " + newStateInstance.GetInstanceID());

            oldState = newStateInstance;
            newStateInstance.ExecuteNoLoopActions(this);
        }
    }
}
