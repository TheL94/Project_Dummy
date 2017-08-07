using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Test.AI
{
    [CreateAssetMenu(menuName = "Test/AI/State/NewState")]
    public class AI_State : ScriptableObject
    {
        public AI_State DefaultTransition = null;
        public List<State_Action> Actions = new List<State_Action>();

        public void Init()
        {
            foreach (State_Action sAction in Actions)
            {
                sAction.Init();
            }
        }

        public void ExecuteNoLoopActions(AI_Controller _controller)
        {
            foreach (State_Action sAction in Actions)
            {
                if (!sAction.LoopAction)
                    sAction.Action.Act(_controller);
            }
        }

        public void ExecuteLoopActions(AI_Controller _controller)
        {
            foreach (State_Action sAction in Actions)
            {
                if(sAction.LoopAction)
                    sAction.Action.Act(_controller);
            }
        }
    }

    [System.Serializable]
    public class State_Action
    {
        public bool LoopAction;
        public AI_Action Action;
        private AI_Action originalAction;
        public AI_State OnCompleteTransition;

        public void Init()
        {
            if (originalAction == null)
                originalAction = Action;

            Action = GameObject.Instantiate(originalAction);
        }
    }
}
