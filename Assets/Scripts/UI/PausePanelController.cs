using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class PausePanelController : UIChanger
    {
        public Button ResumeButton;
        public Button ExitButton;
        UIGamePlayController controller;
        
        public void Init(UIGamePlayController _controller)
        {
            controller = _controller;
            ImageToChange = GetComponentsInChildren<Image>()[1];
        }

        private void OnEnable()
        {
            ResumeButton.onClick.AddListener(() => { GameManager.I.ChageFlowState(Flow.FlowState.Gameplay); });
            ExitButton.onClick.AddListener(() => { GameManager.I.ChageFlowState(Flow.FlowState.ExitGameplay); });
        }

        private void OnDisable()
        {
            ResumeButton.onClick.RemoveAllListeners();
            ExitButton.onClick.RemoveAllListeners();
        }
    }
}