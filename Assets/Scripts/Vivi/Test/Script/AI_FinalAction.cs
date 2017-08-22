﻿using System.Collections.Generic;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.Items;
using Framework.Pathfinding;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/State/NewAction/FinalAction")]
    public class FinalAction : AI_Action
    {
        public Text text;
        protected override bool Act(AI_Controller _controller)
        {
            text = _controller.GetComponentInChildren<Text>();
            text.text = "Fine";
            return true;
        }




    }
}