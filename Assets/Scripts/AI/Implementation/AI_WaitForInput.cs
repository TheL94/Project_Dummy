using UnityEngine.UI;
using UnityEngine;
using DumbProject.UI;
using Framework.AI;

namespace DumbProject.Generic
{
    [CreateAssetMenu(menuName = "AI/NewAction/WaitForInput")]
    public class AI_WaitForInput : AI_Action
    {
        public UIManager.UIButton EndTurn;
        private Button nextTurnButton;
        public KeyCode AlternativeKey;
        private bool isListening;
        private bool inputRecived;

        protected override bool Act(AI_Controller _controller)
        {
            if (nextTurnButton == null)
            {
                WaitKey();
                SetupButton();
            }

            if (!isListening && nextTurnButton!=null)
                ButtonListen();

            if (inputRecived)
            {
                nextTurnButton.onClick.RemoveListener(WaitButton);
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
                LabelContainer lc = button.GetComponent<LabelContainer>();
                if (lc != null && lc.ButtonLabel == EndTurn)
                    nextTurnButton = button;
            }

            if(nextTurnButton != null)
                ButtonListen();
        }

        void ButtonListen()
        {
            nextTurnButton.onClick.AddListener(WaitButton);
            isListening = true;
        }

        void WaitButton()
        {
            inputRecived = true;
        }
    }
}