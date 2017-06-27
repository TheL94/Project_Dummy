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
        public float RotationTime = 0.2f;
        public float TranslationTime = .4f;

        public override void Act(AIController _controller)
        {
            Move(_controller);
        }

        void Move(AIController _controller)
        {
            if (_controller.dumby.nodePath.Count == 0)
                return;

            INetworkable headingNode = _controller.dumby.nodePath[0];

            if(_controller.dumby.currentNode == headingNode)
            {
                _controller.dumby.nodePath.Remove(headingNode);
                return;
            }
            //_controller.transform.DORotateQuaternion(Quaternion.LookRotation(headingNode.spacePosition), RotationTime);
            _controller.transform.DOMove(headingNode.spacePosition, TranslationTime);
        }
    }
}