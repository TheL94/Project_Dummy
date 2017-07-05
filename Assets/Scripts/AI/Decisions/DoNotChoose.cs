using Framework.AI;
using UnityEngine;

namespace DumbProject.AI {
    [CreateAssetMenu(menuName = "AI/State/Decision/NoChoice")]
    public class DoNotChoose : Decision
    {
        public override int Decide(AIController _controller)
        {
            return 0;
        }
    }
}
