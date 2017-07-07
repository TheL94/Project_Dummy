using System.Collections;
using UnityEngine;
using DG.Tweening;
using Framework.AI;
using Framework.Pathfinding;

namespace DumbProject.AI
{
    [CreateAssetMenu(menuName = "AI/State/Action/MoveToNextNode")]
    public class MoveToNextNode : Action
    {
        public override void Act(AIController _controller)
        {
            Move(_controller);
        }

        void Move(AIController _controller)
        {
            (_controller as Generic.AIActor).MoveToNextPathNode();
        }
    }
}