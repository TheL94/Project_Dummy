using Framework.AI;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.AI
{
    [CreateAssetMenu(menuName = "AI/State/Decision/WaitForMovementCompleted")]
    public class WaitForMovementCompleted : Decision
    {
        public override int Decide(AIController _controller)
        {
            IInteractable intercatable = Converter.INetworkableToIInteractable((_controller as AIActor).INetworkableObjective);

            if(_controller.NodePath != null && _controller.NodePath.Count == 0)
            {
                if (intercatable == null)
                    return 1;
                else
                    return 0;
            }
            return 2;
        }
    }
}
