using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class PausePanelController : MonoBehaviour
    {
        public Button ResumeButton;
        public Button ExitButton;
        UIGamePlayController controller;

        public void Init(UIGamePlayController _controller)
        {
            controller = _controller;
        }

        private void OnEnable()
        {
            ResumeButton.onClick.AddListener(() => {
                controller.ActivatePausePanel(false);
                GameManager.I.ChageFlowState(Flow.FlowState.GameplayState);
            });
            ExitButton.onClick.AddListener(() => { controller.QuitGamePlay(); });
        }

        private void OnDisable()
        {
            ResumeButton.onClick.RemoveAllListeners();
            ExitButton.onClick.RemoveAllListeners();
        }
    }
}