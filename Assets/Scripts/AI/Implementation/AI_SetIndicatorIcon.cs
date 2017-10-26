using UnityEngine;
using DumbProject.UI;
using Framework.AI;

namespace DumbProject.Generic
{
    [CreateAssetMenu(menuName = "AI/NewAction/SetIndicatorIcon")]
    public class AI_SetIndicatorIcon : AI_Action
    {
        public IndicatorController.ImageState StateIcon;

        protected override bool Act(AI_Controller _controller)
        {
            (_controller as Dumby).Indicator.ImageCurrentState = StateIcon;
            return true;
        }
    }
}