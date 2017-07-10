using Framework.AI;
using UnityEngine;

namespace DumbProject.AI
{
    [CreateAssetMenu(menuName = "AI/State/Decision/WaitForMovementCompleted")]
    public class WaitForMovementCompleted : Decision
    {
        public override int Decide(AIController _controller)
        {
            if(_controller.nodePath != null && _controller.nodePath.Count == 0)
                return 0;

            return 1;
        }
    }
}
