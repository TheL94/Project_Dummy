using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Test.AI
{
    [CreateAssetMenu(menuName = "Test/AI/State/NewAction/DoPaint")]
    public class AI_DoPaint : AI_Action
    {
        public Color ColorToApply;

        protected override bool Act(AI_Controller _controller)
        {
            SpriteRenderer ctrlRenderer = _controller.GetComponent<SpriteRenderer>();
            ctrlRenderer.color = ColorToApply;
            return true;
        }
    }
}
