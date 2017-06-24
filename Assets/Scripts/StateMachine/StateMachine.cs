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
            State stateToAdd = GetStateByBehaviour(_behaviour);
            if(stateToAdd == null)
                States.Add(new State(_behaviour));
        }

        public void AddState(StateBehaviour _behaviour, StateBehaviour _nextBehaviour)
        {
            AddState(_behaviour);
            State behaviourState = GetStateByBehaviour(_behaviour);
            State stateOfNext = GetStateByBehaviour(_nextBehaviour);
            if (stateOfNext == null)
                AddState(_nextBehaviour);

            stateOfNext = GetStateByBehaviour(_nextBehaviour);

            behaviourState.SetNextState(stateOfNext);
        }

        public void AddState(StateBehaviour _behaviour, List<StateBehaviour> _behaviours)
        {
            AddState(_behaviour);
            SetLinkedStates(GetStateByBehaviour(_behaviour), _behaviours);
        }

        void SetLinkedStates(State _stateToSet, List<StateBehaviour> _behaviours)
        {
            List<StateBehaviour> behaviourToAdd = new List<StateBehaviour>();

            foreach (State behaviourState in _stateToSet.RoutableStates)
            {
                foreach (StateBehaviour behaviourLogic in _behaviours)
                {
                    if (behaviourState.Behaviour == behaviourLogic)
                        behaviourToAdd.Add(behaviourLogic);
                }
            }

            State newToAdd;
            foreach(StateBehaviour newRoute in behaviourToAdd)
            {
                newToAdd = GetStateByBehaviour(newRoute);
                if (newToAdd == null)
                {
                    AddState(newRoute);
                    newToAdd = GetStateByBehaviour(newRoute);
                }
                _stateToSet.RoutableStates.Add(newToAdd);
            }
        }

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
