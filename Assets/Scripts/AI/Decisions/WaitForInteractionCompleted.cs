using Framework.AI;
using UnityEngine;
using DumbProject.Generic;
using DumbProject.Rooms;

namespace DumbProject.AI
{
    [CreateAssetMenu(menuName = "AI/State/Decision/WaitForInteractionCompleted")]
    public class WaitForInteractionCompleted : Decision
    {
        public override int Decide(AIController _controller)
        {
            IInteractable objective = Converter.INetworkableToIInteractable((_controller as AIActor).INetworkableObjective);
            if(objective != null && !objective.IsInteractable)
                return 0;

            return 1;
        }
    }
}
