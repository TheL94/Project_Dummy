using System.Collections.Generic;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.Items;
using Framework.Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/State/NewAction/FirstAction")]
    public class FirstAction : AI_Action
    {
        public Text text;
        protected override bool Act(AI_Controller _controller)
        {
            text = _controller.GetComponentInChildren<Text>();
            text.text = "premi spazio";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                return true; 
            }
            return false;
        }

       


    }
}
