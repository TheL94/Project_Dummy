using Framework.AI;
using UnityEngine;

namespace DumbProject.Generic
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