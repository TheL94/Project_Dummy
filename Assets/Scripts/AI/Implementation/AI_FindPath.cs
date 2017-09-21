using DumbProject.Generic;
using Framework.Pathfinding;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/NewAction/FindPath")]
    public class AI_FindPath : AI_Action
    {
        protected override bool Act(AI_Controller _controller)
        {
            INetworkable start = (_controller as IPathfinder).CurrentNetworkable;
            INetworkable target = (_controller as IPathfinder).Objective;

            INetworkable[] newPath = Pathfinder.FindPath(target, start).ToArray();
            if (newPath != null && newPath.Length > 0)
            {
                (_controller as IPathfinder).Path.SetOriginalPath(newPath);
                return true;
            }
            else if(start.spacePosition == target.spacePosition)
            {
                (_controller as IPathfinder).Path.SetOriginalPath(new INetworkable[] { target });
                return true;
            }
            else
                return false;
        }
    }
}
