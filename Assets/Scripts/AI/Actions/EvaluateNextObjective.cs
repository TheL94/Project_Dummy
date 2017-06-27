using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AI;
using Framework.Pathfinding;

namespace DumbProject.AI
{
    [CreateAssetMenu(menuName = "AI/State/Action/EvaluateNextObjective")]
    public class EvaluateNextObjective : Action
    {
        INetworkable objectiveNode { get { return Generic.GameManager.I.TestNode; } }
        Pathfinder pathfinder;

        public override void Act(AIController _controller)
        {
            Evaluate(_controller);
        }

        void Evaluate(AIController _controller)
        {
            if (pathfinder == null)
                pathfinder = _controller.dumby.pathFinder;

            if(_controller.dumby.nodePath.Count == 0 && _controller.dumby.currentNode != objectiveNode)
                _controller.dumby.nodePath= pathfinder.FindPath(objectiveNode);
        }
    }
}
