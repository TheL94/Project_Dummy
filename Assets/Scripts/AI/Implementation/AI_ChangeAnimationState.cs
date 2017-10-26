using Framework.AI;
using UnityEngine;

namespace DumbProject.Generic
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
