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

        #region Pathfinder
        public virtual INetworkable CurrentNode { get; set; }
        public List<INetworkable> NodePath
        {
            get;
            set;
        }
        #endregion
    }
}
