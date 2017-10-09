using System.Collections.Generic;
using UnityEngine;

namespace Framework.AI
{
    /// <summary>
    /// Class the add some property to the normal Action in order to make it easy manageable by the Unity Editor and State class.
    /// </summary>
    [System.Serializable]
    public class ActionStructureForState
    {
        public float Delay;
        private float timer;
        public bool Loop;
        private bool runtOnce;
        public AI_Action Action;
        public AI_State OnCompleteTransition;

        private AI_Action originalAction;

        public void Init(bool toSetUp)
        {
            if (toSetUp)
            {
                if (originalAction == null)
                    originalAction = Action;

                Action = GameObject.Instantiate(originalAction);
            }

            if (Delay > 0)
            {
                if (!Loop)
                    runtOnce = false;

                timer = Delay;
            }

            //if (Action.OnEnd != null)
                Action.OnEnd += HandleOnEnd;
        }

        public void Clean()
        {
            //if (Action.OnEnd != null)
                Action.OnEnd -= HandleOnEnd;
        }

        public void Run(AI_Controller _controller)
        {
            if (timer <= 0)
            {
                if (Loop || (runtOnce == false))
                {
                    Action.DoAct(_controller);
                    runtOnce = true;
                }
            }
            else
                timer -= Time.deltaTime;
        }

        void HandleOnEnd(AI_Controller _controller)
        {
            if (OnCompleteTransition != null)
                _controller.CurrentState = OnCompleteTransition;

        }
    }
}
