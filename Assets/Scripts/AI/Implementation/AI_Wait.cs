using System.Collections.Generic;
using UnityEngine;
using System;


namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/NewAction/Wait")]
    public class AI_Wait : AI_Action
    {
        Timer timer;
        public float Delay;
        protected override bool Act(AI_Controller _controller)
        {
            if (!timer)
            {
				
				if (_controller.gameObject.GetComponent<Timer>() == null)
                    timer = _controller.gameObject.AddComponent<Timer>();
                else timer = _controller.GetComponent<Timer>();

                timer.TimeToWait = Delay;
            }

            timer.Active = true;
            if (timer.IsOver)
            {				
				timer.Active = false;
                return true;                
            }
               
            return false;
        }

    }
}