using System.Collections.Generic;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.Items;
using Framework.Pathfinding;
using UnityEngine;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/NewAction/LooseTheGame")]
    public class AI_LooseTheGame : AI_Action
    {
        protected override bool Act(AI_Controller _controller)
        {
            GameManager.I.GameLost();
            return true;
        }
    }
}