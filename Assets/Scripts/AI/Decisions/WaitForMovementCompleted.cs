using Framework.AI;
using UnityEngine;

namespace DumbProject.AI
{
    [CreateAssetMenu(menuName = "AI/State/Decision/WaitForMovementCompleted")]
    public class WaitForMovementCompleted : Decision
    {
        public override int Decide(AIController _controller)
        {
            if(_controller.NodePath != null && _controller.NodePath.Count == 0)
                return 0;

            return 1;
        }
    }
}
