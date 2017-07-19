using System.Collections.Generic;
using DumbProject.Grid;
using UnityEngine;
using Framework.AI;
using Framework.Pathfinding;
using DumbProject.Generic;
using DumbProject.Items;
using DumbProject.Rooms;

namespace DumbProject.AI
{
    [CreateAssetMenu(menuName = "AI/State/Action/FindPath")]
    public class FindPath : Action
    {
        GridController grid;

        public override void Act(AIController _controller)
        {
            if (grid == null)
                grid = (_controller as AIActor).Grid;

            Evaluate(_controller);
        }

        /// <summary>
        /// Trova un percorso verso l'obiettivo se non c'è già
        /// </summary>
        /// <param name="_controller"></param>
        void Evaluate(AIController _controller)
        {
            List<INetworkable> path = _controller.NodePath;
            if (path != null && path.Count > 0/* && path[path.Count - 1] == objectivePosition*/)
                return;

            INetworkable objectivePosition = (_controller as AIActor).INetworkableObjective;

            if(objectivePosition == null)
            {
                Debug.LogWarning("Evaluating a target that is neighter a ItemGeneric nor a Edge");
                Debug.LogWarning("Trying to find a path on a null NextObjectivePosition");
                return;
            }
            List<INetworkable> newPath = Pathfinder.FindPath(objectivePosition, (_controller as AIActor).CurrentNode);
            (_controller as AIActor).SetPath(newPath);
        }
    }
}
