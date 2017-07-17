using Framework.AI;
using UnityEngine;
using DumbProject.Generic;
using DG.Tweening;

namespace DumbProject.AI
{
    [CreateAssetMenu(menuName = "AI/State/Decision/WaitForMovementCompleted")]
    public class WaitForMovementCompleted : Decision
    {
        public override int Decide(AIController _controller)
        {
            if(!(_controller as AIActor).pathTrack.IsActive())
                return 0;

            return 1;
        }
    }
}
