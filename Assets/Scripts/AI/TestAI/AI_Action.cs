using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Test.AI
{
    public abstract class AI_Action : ScriptableObject
    {
        public void DoAct(AI_Controller _controller)
        {
            if (Act(_controller))
                if(OnEnd != null)
                    OnEnd(_controller);
        }

        protected abstract bool Act(AI_Controller _controller);

        public delegate void OnLifeFlow(AI_Controller _controller);
        public event OnLifeFlow OnEnd;
    }
}
