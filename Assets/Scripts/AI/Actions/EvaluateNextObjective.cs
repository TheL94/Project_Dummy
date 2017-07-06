using System.Collections;
using DumbProject.Grid;
using UnityEngine;
using Framework.AI;
using Framework.Pathfinding;
using DumbProject.Generic;

namespace DumbProject.AI
{
    [CreateAssetMenu(menuName = "AI/State/Action/EvaluateNextObjective")]
    public class EvaluateNextObjective : Action
    {
        Pathfinder pathfinder;
        GridController grid { get { return GameManager.I.MainGridCtrl; } }

        public override void Act(AIController _controller)
        {
            Evaluate(_controller);
        }

        void Evaluate(AIController _controller)
        {
            if ((_controller as AIActor).nextRoomObjective == null)
                return;

            if (pathfinder == null)
                pathfinder = _controller.pathFinder;

            if ((_controller.nodePath == null || _controller.nodePath.Count == 0) && _controller.CurrentNode != (_controller as AIActor).nextRoomObjective)
                _controller.nodePath = pathfinder.FindPath(grid.GetSpecificGridNode((_controller as AIActor).nextRoomObjective.transF.position), (_controller as AIActor).CurrentNode);
        }
    }
}
