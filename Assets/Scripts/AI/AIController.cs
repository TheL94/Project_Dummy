using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace Framework.AI
{
    public class AIController : MonoBehaviour
    {
        public State CurrentState;
        [HideInInspector]
        public Dumby dumby;
        public bool IsInDebugMode;
        bool isActive;

        public void Setup(Dumby _dumby, bool _setActive = true)
        {
            dumby = _dumby;
            isActive = _setActive;
        }

        private void Update()
        {
            if (!isActive)
                return;
            CurrentState.UpdateState(this);
        }

        public void TransitionToState(State _nextState)
        {
            if (_nextState != CurrentState)
            {
                CurrentState = _nextState;
            }
        }

        private void OnDrawGizmos()
        {
            if (!IsInDebugMode)
                return;
            if(CurrentState != null)
            {
                Gizmos.color = CurrentState.StateColor;
                Gizmos.DrawSphere(transform.position, 2f);
            }
        }
    }
}
