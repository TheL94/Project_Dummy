using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using Framework.AI;

namespace DumbProject.AI
{
    [CreateAssetMenu(menuName = "AI/State/Action/WaitInIdle")]
    public class WaitInIdle : Action
    {
        public override void Act(AIController _controller)
        {
            (_controller as Dumby).AnimState = Dumby.AnimationState.Idle;
        }
    }
}
