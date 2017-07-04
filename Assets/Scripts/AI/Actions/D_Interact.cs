using Framework.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.AI
{
    [CreateAssetMenu(menuName = "AI/State/Action/D_Interact")]
    public class D_Interact : Action
    {
        public override void Act(AIController _controller)
        {
            if((_controller as Dumby).nextRoomObjective != null)
            {
                Destroy((_controller as Dumby).nextRoomObjective.transF.gameObject);
                (_controller as Dumby).nextRoomObjective = null;
            }
        }
    }
}