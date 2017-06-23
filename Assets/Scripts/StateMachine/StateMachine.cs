using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.StateMachine {
    public class StateMachine
    {
        List<State> States;


        public void Setup() { }
        public void Init() { }

        public void AddState(StateBehaviour _behaviour)
        {
            foreach (State state in States)
            {
                if (state.Behaviour == _behaviour)
                    return;
            }

            States.Add(new State(_behaviour));
        }
        public void AddState(StateBehaviour _behaviour, StateBehaviour _nextBehaviour)
        {
            foreach (State state in States)
            {
                if (state.Behaviour == _behaviour)
                {
                    if (state.NextState.Behaviour == _nextBehaviour)
                        return;
                    else
                    {
                        State stateOfNext = GetStateByBehaviour(_nextBehaviour);
                        state.SetNextState(stateOfNext != null ? stateOfNext : new State(_nextBehaviour));
                    }
                }
            }
            States.Add(new State(_behaviour));
        }
        public void AddState(StateBehaviour _behaviour, List<StateBehaviour> _behaviours)
        {
            foreach (State state in States)
            {
                if (state.Behaviour == _behaviour)
                    return;
            }

            States.Add(new State(_behaviour));
        }

        //SetLinkedStates(State _stateToSet, )
        State GetStateByBehaviour(StateBehaviour _behavour)
        {
            foreach (State state in States)
            {
                if (state.Behaviour == _behavour)
                    return state;
            }
            return null;
        }

        class State
        {
            public StateBehaviour Behaviour { get; private set; }

            private bool IsBypassingRules;
            private State _nextState;
            public State NextState
            {
                get {
                    if (_nextState == null)
                        _nextState = RoutableStates[0];

                    return _nextState;
                    if (!IsBypassingRules)
                        _nextState = RoutableStates[0];
                }
                private set { _nextState = value; }
            }

            public List<State> RoutableStates = new List<State>();
            
            public State(StateBehaviour _behaviour)
            {
                Behaviour = _behaviour;
            }

            public void ResetRules()
            {
                IsBypassingRules = false;
            }

            public void SetNextState(State _next, bool _isBypassinngRules = false)
            {
                IsBypassingRules = _isBypassinngRules;
                NextState = _next;

                if (RoutableStates.Count == 0)
                    RoutableStates.Add(_next);
            }
        }
    }
}
