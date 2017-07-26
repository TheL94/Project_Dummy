using DG.Tweening;
using UnityEngine.UI;
using Framework.AI;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.AI {
    [CreateAssetMenu(menuName = "AI/State/Action/PopUp")]
    public class PopUp : Action
    {
        public Sprite Icon;
        public float VerticalOffSet;
        public float VerticalJumpHeight;
        public float DisplayTime;

        GameObject actualPopUp;
        SpriteRenderer popUpRenderer;

        public override void Act(AIController _controller)
        {
            AIActor actor = _controller as AIActor;

            PopUpSetup(actor);
            ShowPopUp(actor);
        }

        void ShowPopUp(AIActor _actor)
        {
            actualPopUp.transform.DOJump(actualPopUp.transform.position, VerticalJumpHeight, 1, DisplayTime / 2)
                .SetDelay(DisplayTime/2)
                .OnComplete(()=> { actualPopUp.SetActive(false); });
        }

        void PopUpSetup(AIActor _actor)
        {
            if (actualPopUp == null)
                actualPopUp = new GameObject("PopUp");
            else
                actualPopUp.SetActive(true);

            if (popUpRenderer == null)
                popUpRenderer = actualPopUp.AddComponent<SpriteRenderer>();
            else
                popUpRenderer.enabled = true;

            popUpRenderer.sprite = Icon;
            actualPopUp.transform.parent = _actor.transform;
            actualPopUp.transform.position = _actor.transform.position + new Vector3(0, VerticalOffSet,0);

        }
    }
}
