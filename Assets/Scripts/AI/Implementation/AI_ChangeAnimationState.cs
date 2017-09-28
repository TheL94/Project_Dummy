using DumbProject.Generic;
using UnityEngine;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/NewAction/ChangeAnimationState")]
    public class AI_ChangeAnimationState : AI_Action
    {
        public Dumby.AnimationState AnimationState;

        protected override bool Act(AI_Controller _controller)
        {
            (_controller as Dumby).AnimState = AnimationState;

            return true;
        }
    }
}
