using UnityEngine;
using DumbProject.UI;
using Framework.AI;

namespace DumbProject.Generic
{
    [CreateAssetMenu(menuName = "AI/NewAction/SetIndicatorIcon")]
    public class AI_SetIndicatorIcon : AI_Action
    {
        public IndicatorController.ImageState StateIcon;
        private IndicatorController indicator;

        protected override bool Act(AI_Controller _controller)
        {
            if (indicator == null)
                indicator = (_controller as Dumby).Indicator;
            else
                indicator.ImageCurrentState = StateIcon;
            return true;
        }
    }
}