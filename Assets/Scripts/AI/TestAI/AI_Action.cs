using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Test.AI
{

    [CreateAssetMenu(menuName = "Test/AI/State/NewAction")]
    public abstract class AI_Action : ScriptableObject
    {
        public void DoAct(AI_Controller _controller)
        {
            Act(_controller);
            OnEnd(_controller);
        }

        protected abstract void Act(AI_Controller _controller);

        public delegate void OnLifeFlow(AI_Controller _controller);
        public event OnLifeFlow OnEnd;
    }
}
