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

        protected bool isToSetUp = false;

        public void Init()
        {
            foreach (State_Action sAction in Actions)
            {
                sAction.Init(isToSetUp);
            }
        }

        public void Clean()
        {
            foreach (State_Action sAction in Actions)
            {
                sAction.Clean();
            }
        }

        public void ExecuteNoLoopActions(AI_Controller _controller)
        {
            foreach (State_Action sAction in Actions)
            {
                if (!sAction.LoopAction)
                    sAction.Action.DoAct(_controller);
            }
        }

        public void ExecuteLoopActions(AI_Controller _controller)
        {
            foreach (State_Action sAction in Actions)
            {
                if(sAction.LoopAction)
                    sAction.Action.DoAct(_controller);
            }
        }
    }

    [System.Serializable]
    public class State_Action
    {
        public bool LoopAction;
        public AI_Action Action;
        public AI_State OnCompleteTransition;

        private AI_Action originalAction;
        private AI_Action.OnLifeFlow onEndHandler;

        public void Init(bool toSetUp)
        {
            if (toSetUp)
            {
                if (originalAction == null)
                    originalAction = Action;

                Action = GameObject.Instantiate(originalAction);
                onEndHandler = new AI_Action.OnLifeFlow(HandleOnEnd);
            }

            if (onEndHandler != null)
                Action.OnEnd += onEndHandler;
        }

        public void Clean()
        {
            if (onEndHandler != null)
                Action.OnEnd -= onEndHandler;
        }

        ~State_Action()
        {
            Clean();
        }

        void HandleOnEnd(AI_Controller _controller)
        {
            if(OnCompleteTransition != null)
                _controller.CurrentState = OnCompleteTransition;
        }
    }
}
