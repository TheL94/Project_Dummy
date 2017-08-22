using System.Collections.Generic;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.Items;
using Framework.Pathfinding;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/State/NewAction/Wait")]
    public class AI_Wait : AI_Action
    {
        #region test
        public Text text;
        #endregion
        public Timer timer;
        public float Delay;
        protected override bool Act(AI_Controller _controller)
        {
            text = _controller.GetComponentInChildren<Text>();
            if (!timer)
            {
                //timer = GameManagerTest.I.timer; 
                //oppure
                if (_controller.gameObject.GetComponent<Timer>() == null)
                    timer = _controller.gameObject.AddComponent<Timer>();
                else timer = _controller.GetComponent<Timer>();
                timer.TimeToWait = Delay;
            }

            #region test
            if (text)
            {
                text.text = "WAIT " + (int)timer.time;
            }
            #endregion

            timer.Active = true;
            if (timer.IsOver)
                return true;
            else return false;
        }


    }
}