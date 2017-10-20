using UnityEngine.UI;
using UnityEngine;
using DumbProject.UI;
using DumbProject.Generic;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/NewAction/WaitForInput")]
    public class AI_WaitForInput : AI_Action
    {
        public UIManager.UIButton EndTurn;
        private Button nextTurn;
        public KeyCode AlternativeKey;
        private bool isListening;
        private bool inputRecived;

        protected override bool Act(AI_Controller _controller)
        {
            if (nextTurn == null)
            {
                WaitKey();
                SetupButton();
            }

            if (!isListening && nextTurn!=null)
                ButtonListen();

            if (inputRecived)
            {
                nextTurn.onClick.RemoveListener(WaitButton);
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

        void SetupButton()
        {
            Button[] buttons = FindObjectsOfType<Button>();
            foreach (Button button in buttons)
            {
                if(button.GetComponent<LabelContainer>().ButtonLabel == EndTurn)
                    nextTurn = button;
            }

            if(nextTurn != null)
                ButtonListen();
        }

        void ButtonListen()
        {
            nextTurn.onClick.AddListener(WaitButton);
            isListening = true;
        }

        void WaitButton()
        {
            inputRecived = true;
        }

    }
}