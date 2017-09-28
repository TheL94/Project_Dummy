using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.AI
{
    /// <summary>
    /// It runs and controls the flow of the current State.
    /// Also manage the state change.
    /// </summary>
    public class AI_Controller : MonoBehaviour
    {
        public bool IsActive = false;
        public AI_State InitialDefaultState;
        private AI_State _currentState;
        public AI_State CurrentState
        {
            get { return _currentState; }
            set { _currentState = OnCurrentStateSet(CurrentState, value);

			}
        }

        public void Init(AI_State currentState)
        {
            CurrentState = currentState;
            OnInit();
        }

        protected virtual void OnInit() { }

        private void Update()
        {
            if(CurrentState == null)
                Init(InitialDefaultState);

            if (IsActive)
            {
                CurrentState.Run(this);
            }
        }

        /// <summary>
        /// Called on CurrentState Set to manage the shift correctly
        /// </summary>
        /// <param name="oldState">Previus CurrentState</param>
        /// <param name="newState">Incoming State to Set</param>
        private AI_State OnCurrentStateSet(AI_State oldState, AI_State newState)
        {
            if(oldState != null)
                oldState.Clean(); //Clean the old State as soon as the state change in order to prevent multiple State changes called by events

            AI_State newStateInstance = AI_DataManager.GetState(this, newState);
            newStateInstance.Init();
            Debug.Log(newStateInstance.name + " instance: " + newStateInstance.GetInstanceID()); //TODO: cut this line off once finished testing phase

            return newStateInstance;
        }
    }
}
