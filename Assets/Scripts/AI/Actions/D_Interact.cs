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
            if((_controller as AIActor).NextRoomObjective != null)
            {
                Destroy((_controller as AIActor).NextRoomObjective.transF.gameObject);
                (_controller as AIActor).NextRoomObjective = null;
            }
        }
    }
}