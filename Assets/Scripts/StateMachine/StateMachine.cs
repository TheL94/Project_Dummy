using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.StateMachine
{
    public class StateMachine
    {
        /// <summary>
        /// List of state in the StateMachine
        /// </summary>
        List<State> States;

        #region API
        /// <summary>
        /// Add a State to the StateMachine as Default
        /// </summary>
        /// <param name="_behaviour">StateBehaviour to Add</param>
        public void AddState(StateBehaviour _behaviour)
        {
            AddState(_behaviour);
        }
        /// <summary>
        /// Add a State to the StateMachine
        /// </summary>
        /// <param name="_behaviour">StateBehaviour to add</param>
        /// <param name="_nextBehaviour">StateBehaviour to set as default next state on end of this one</param>
        public void AddState(StateBehaviour _behaviour, StateBehaviour _nextBehaviour)
        {
            AddNewState(_behaviour, _nextBehaviour);
        }
        /// <summary>
        /// Add a State to the StateMachine
        /// </summary>
        /// <param name="_behaviour">StateBehaviour to Add</param>
        /// <param name="_behaviours">List of other StateBehaviours to add. They will be added as default with no link to other states</param>
        public void AddState(StateBehaviour _behaviour, List<StateBehaviour> _behaviours)
        {
            AddNewState(_behaviour, _behaviours);
        }
        /// <summary>
        /// Remove a State from the states of the StateMachine
        /// </summary>
        /// <param name="_behaviour">State to remove</param>
        public void RemoveState(StateBehaviour _behaviour)
        {
            State stateToRemove = GetStateByBehaviour(_behaviour);
            States.Remove(stateToRemove);
            foreach (State state in States)
            {
                if (state.RoutableStates.Contains(stateToRemove))
                    state.RoutableStates.Remove(stateToRemove);
            }
        }
        /// <summary>
        /// Sum the new given behaviours to the possible routes of this State
        /// </summary>
        /// <param name="_behaviour">The state to Update (create new if it doesn't exist)</param>
        /// <param name="_behaviours">List of StateBehaviours to add</param>
        public void UpdateStateRoutes(StateBehaviour _behaviour, List<StateBehaviour> _behaviours)
        {
            State stateToUpdate = GetStateByBehaviour(_behaviour);
            if (stateToUpdate == null)
                stateToUpdate = AddNewState(_behaviour, _behaviours);
            else
                UpdateLinkedStates(stateToUpdate, _behaviours);
        }
        /// <summary>
        /// Replace the possible routes of the StateBehaviour
        /// </summary>
        /// <param name="_behaviour">The state to Update (create new if it doesn't exist)</param>
        /// <param name="_behaviours">List of StateBehaviours to set as new</param>
        public void SetStateRoutes(StateBehaviour _behaviour, List<StateBehaviour> _behaviours)
        {
            State stateToUpdate = GetStateByBehaviour(_behaviour);
            if (stateToUpdate == null)
                stateToUpdate = AddNewState(_behaviour, _behaviours);
            else
                SetLinkedStates(stateToUpdate, _behaviours);
        }
        #endregion

        #region Statemachine Setup and Init
        State AddNewState(StateBehaviour _behaviour)
        {
            State stateToAdd = GetStateByBehaviour(_behaviour);
            if (stateToAdd == null)
                States.Add(new State(_behaviour));
            return stateToAdd;
        }

        State AddNewState(StateBehaviour _behaviour, StateBehaviour _nextBehaviour)
        {
            State addedState = AddNewState(_behaviour);
            State stateOfNext = GetStateByBehaviour(_nextBehaviour);
            if (stateOfNext == null)
                stateOfNext = AddNewState(_nextBehaviour);

            addedState.SetNextState(stateOfNext);

            return addedState;
        }

        State AddNewState(StateBehaviour _behaviour, List<StateBehaviour> _behaviours)
        {
            State addedState = AddNewState(_behaviour);
            UpdateLinkedStates(addedState, _behaviours);
            return addedState;
        }

        void UpdateLinkedStates(State _stateToSet, List<StateBehaviour> _behaviours)
        {
            List<StateBehaviour> behaviourToAdd = new List<StateBehaviour>();

            foreach (StateBehaviour behaviourLogic in _behaviours)
            {
                if (!_stateToSet.RoutableStates.Contains(GetStateByBehaviour(behaviourLogic)))
                    behaviourToAdd.Add(behaviourLogic);
            }

            State newToAdd;
            foreach (StateBehaviour newRoute in behaviourToAdd)
            {
                newToAdd = GetStateByBehaviour(newRoute);
                if (newToAdd == null)
                {
                    newToAdd = AddNewState(newRoute);
                }
                _stateToSet.RoutableStates.Add(newToAdd);
            }
        }

        void SetLinkedStates(State _stateToSet, List<StateBehaviour> _behaviours)
        {
            List<State> statesToSet = new List<State>();
            foreach (StateBehaviour sB in _behaviours)
            {
                State stateToAdd = GetStateByBehaviour(sB);
                if (stateToAdd == null)
                {
                    stateToAdd = AddNewState(sB);
                    States.Add(stateToAdd);
                }
                statesToSet.Add(stateToAdd); 
            }

            _stateToSet.RoutableStates = statesToSet;
        }
        #endregion

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
                get
                {
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
