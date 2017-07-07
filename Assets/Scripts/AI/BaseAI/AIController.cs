using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Pathfinding;

namespace Framework.AI
{
    public class AIController : MonoBehaviour
    {
        State _currentState;
        public State CurrentState
        {
            get
            {
                if (_currentState == null)
                    _currentState = InitialState;
                return _currentState;
            }
            set
            {
                _currentState = value;
                Debug.Log(CurrentState.name);
            }
        }
        public State InitialState;
        public bool DebugMode;
        public bool isActive = false;

        public virtual void Setup(bool _setActive = true)
        {
            isActive = _setActive;
        }

        private void Update()
        {
            if (!isActive)
                return;
            CurrentState.UpdateState(this);
            OnUpdate();
        }

        protected virtual void OnUpdate() { }

        public void TransitionToState(State _nextState)
        {
            if (_nextState != CurrentState)
            {
                CurrentState = _nextState;
            }
        }

        private void OnDrawGizmos()
        {
            if (!DebugMode)
                return;
            if (CurrentState != null)
            {
                Gizmos.color = CurrentState.StateColor;
                Gizmos.DrawSphere(transform.position, 0.2f);
            }
        }


        #region Pathfinder
        public virtual INetworkable CurrentNode { get; set; }
        public List<INetworkable> nodePath = new List<INetworkable>();
        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            foreach (INetworkable node in nodePath)
            {
                Gizmos.DrawCube(node.spacePosition, Vector3.one);
            }
        }
    }
}
