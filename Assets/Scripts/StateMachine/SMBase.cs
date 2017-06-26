using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.SM {
    public class SMBase : MonoBehaviour
    {
        private List<StateBase> _states;
        public List<StateBase> States
        {
            get { return _states; }
            private set { _states = value; }
        }

        private State _currentState;

        public State CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }


        private void Update()
        {
            CurrentState.state.Run();
        }

        public struct State
        {
            public StateBase state;
            public StateBase nextState; 
        }
    }

}
