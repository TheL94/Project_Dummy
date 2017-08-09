using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Test.AI
{
    [CreateAssetMenu(menuName = "Test/AI/State/NewAction/DoScale")]
    public class AI_DoScale : AI_Action
    {
        public float ScaleMultiplier;

        protected override bool Act(AI_Controller _controller)
        {
            Transform ctrlTransform = _controller.GetComponent<Transform>();
            ctrlTransform.localScale = Vector3.one * ScaleMultiplier;
            return true;
        }
    }
}