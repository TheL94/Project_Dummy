using System.Collections.Generic;
using DumbProject.Grid;
using UnityEngine;
using Framework.AI;
using Framework.Pathfinding;
using DumbProject.Generic;
using DumbProject.Items;

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
            INetworkable objectivePosition = (_controller as AIActor).NextObjectivePosition;
            if(objectivePosition == null)
            {
                Debug.LogWarning("Trying to find a path on a null NextObjectivePosition");
                return;
            }

            List<INetworkable> path = _controller.nodePath;

            if (path != null && path.Count > 0 && path[path.Count - 1] == objectivePosition)
                return;

            _controller.nodePath = Pathfinder.FindPath(objectivePosition, (_controller as AIActor).CurrentNode);
        }
    }
}
