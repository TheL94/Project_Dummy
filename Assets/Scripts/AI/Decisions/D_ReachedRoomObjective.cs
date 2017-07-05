using System.Collections;
using UnityEngine;
using Framework.AI;
using DumbProject.Generic;

namespace DumbProject.AI {
    [CreateAssetMenu(menuName = "AI/State/Decision/ReachRoomObjective")]
    public class D_ReachedRoomObjective : Decision
    {
        public override int Decide(AIController _controller)
        {
            if (Vector3.Distance(_controller.transform.position, (_controller as AIActor).nextRoomObjective.transF.position) <= (_controller as AIActor).InteractionRadius)
                return 1;
            return 0;
        }
    }
}
