using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject;
using DumbProject.GDR_System;
using Framework.Pathfinding;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/NewAction/WaitForInput")]
    public class AI_WaitForInput : AI_Action
    {
        public KeyCode Key;
        protected override bool Act(AI_Controller _controller)
        {
            if (Input.GetKeyDown(Key))
                return true;
            return false;
        }
    }
}