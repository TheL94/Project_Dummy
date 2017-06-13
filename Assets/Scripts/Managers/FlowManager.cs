using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;


namespace DumbProject.Flow
{
    public class FlowManager : MonoBehaviour
    {

        private FlowState _currentState;

        public FlowState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }



    }

    public enum FlowState
    {
        MenuState,
        GameplayState,
    }
}