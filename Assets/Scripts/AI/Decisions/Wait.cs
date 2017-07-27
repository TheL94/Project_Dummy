using Framework.AI;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.Items;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DumbProject.AI
{
    [CreateAssetMenu(menuName = "AI/State/Decision/Wait")]
    public class Wait : Decision
    {
        public float TimeToWait = 0;

        public override int Decide(AIController _controller)
        {

            Timer timer = _controller.GetComponent<Timer>();
            if (timer == null)
            {
                timer = _controller.gameObject.AddComponent<Timer>();
                timer.Setup(_controller.gameObject, TimeToWait);
                _controller.InhibitStateChange(true);
            }

            if (timer.IsOver)
            {
                _controller.InhibitStateChange(false);
                Destroy(timer);
            }

            return 0;
        }
    }
}
