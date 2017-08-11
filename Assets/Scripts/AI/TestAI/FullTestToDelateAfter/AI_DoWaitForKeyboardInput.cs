using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Test.AI
{
    [CreateAssetMenu(menuName = "Test/AI/State/NewAction/DoWaitForInput")]
    public class AI_DoWaitForKeyboardInput : AI_Action
    {
        public KeyCode InputToWaitFor;

        protected override bool Act(AI_Controller _controller)
        {
            if (Input.GetKeyDown(InputToWaitFor))
                return true;
            else
                return false;
        }
    }
}
