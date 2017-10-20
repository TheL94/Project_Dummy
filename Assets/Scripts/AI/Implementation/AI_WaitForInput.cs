using UnityEngine.UI;
using UnityEngine;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/NewAction/WaitForInput")]
    public class AI_WaitForInput : AI_Action
    {
        public Button NextTurn;
        public KeyCode AlternativeKey;
        private bool isListening;
        private bool inputRecived;

        protected override bool Act(AI_Controller _controller)
        {
            if (NextTurn == null)
                WaitKey();

            if (!isListening)
                NextTurn.onClick.AddListener(WaitButton);

            if (inputRecived)
            {
                NextTurn.onClick.RemoveListener(WaitButton);
                isListening = false;

                inputRecived = false;
                return true;
            }

            return false;
        }

        void WaitKey()
        {
            if (Input.GetKeyDown(AlternativeKey))
                inputRecived = true;
        }

        void WaitButton()
        {
            inputRecived = true;
        }

    }
}