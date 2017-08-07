using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Test.AI
{
    public class AI_Controller : MonoBehaviour
    {
        public bool IsActive = true;
        public AI_State CurrentState;

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
    }
}
