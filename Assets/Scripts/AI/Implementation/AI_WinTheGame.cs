using Framework.AI;
using UnityEngine;

namespace DumbProject.Generic
{
    [CreateAssetMenu(menuName = "AI/NewAction/WinTheGame")]
    public class AI_WinTheGame : AI_Action
    {
        protected override bool Act(AI_Controller _controller)
        {
            GameManager.I.GameWon();
            return true;
        }
    }
}
