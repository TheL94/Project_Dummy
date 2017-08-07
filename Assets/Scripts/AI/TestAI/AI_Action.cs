using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Test.AI
{
    [CreateAssetMenu(menuName = "Test/AI/State/NewAction")]
    public abstract class AI_Action : ScriptableObject
    {
        public abstract void Act(AI_Controller _controller);
    }
}
