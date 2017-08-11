using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/State/NewAction/ChooseObjective")]
    public class AI_ChooseObjective : AI_Action
    {
        protected override bool Act(AI_Controller _controller)
        {
            return true;
        }
    }
}


