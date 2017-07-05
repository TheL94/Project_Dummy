using System.Collections;
using UnityEngine;
using DG.Tweening;
using Framework.AI;
using DumbProject.Generic;

namespace DumbProject.AI
{
    [CreateAssetMenu(menuName = "AI/State/Action/ReachRoomObjective")]
    public class ReachRoomObjective : Action
    {
        public override void Act(AIController _controller)
        {
            Move(_controller);
        }

        void Move(AIController _controller)
        {
            (_controller as Dumby).FollowPath();
        }
    }
}
