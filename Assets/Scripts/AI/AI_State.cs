using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/NewState")]
    public class AI_State : ScriptableObject
    {
        public List<ActionStructureForState> Actions = new List<ActionStructureForState>();

        protected bool isToSetUp = true;

        /// <summary>
        /// Initialize the state with starting settings
        /// </summary>
        public void Init()
        {
            foreach (ActionStructureForState sAction in Actions)
            {
                sAction.Init(isToSetUp);
            }
            isToSetUp = false;
        }

        /// <summary>
        /// Clean Actions instances in order to reset
        /// </summary>
        public void Clean()
        {
            foreach (ActionStructureForState sAction in Actions)
            {
                sAction.Clean();
            }
        }
        /// <summary>
        /// Executions of all the action, menaging the loops
        /// </summary>
        /// <param name="_controller"></param>
        public void Run(AI_Controller _controller)
        {
            foreach (ActionStructureForState sAction in Actions)
            {
                sAction.Run(_controller);
            }
        }
    }

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

            if(Delay > 0)
            {
                if (!Loop)
                    runtOnce = false;
                

                timer = Delay;
            }

            if (onEndHandler != null)
                Action.OnEnd += onEndHandler;
        }

        public void Clean()
        {
            if (onEndHandler != null)
                Action.OnEnd -= onEndHandler;
        }

        public void Run(AI_Controller _controller)
        {
            if (timer <= 0)
            {
                if(Loop || (runtOnce == false))
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
