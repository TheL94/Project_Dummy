using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class PausePanelController : MonoBehaviour, IUIChanger
    {
        public Button ResumeButton;
        public Button ExitButton;
        UIGamePlayController controller;

        Image gamePlayImage;

        public Sprite VerticalUI;
        public Sprite HorizontalUI;

        public void Init(UIGamePlayController _controller)
        {
            controller = _controller;
            gamePlayImage = GetComponent<Image>();
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

        public void SetUIOrientation(ScreenOrientation _orientation)
        {
            if (GameManager.I.DeviceEnvironment == DeviceType.Desktop)
            {
                if (GameManager.I.UIMng.ForceVerticalUI)
                    gamePlayImage.sprite = VerticalUI;
                else
                    gamePlayImage.sprite = HorizontalUI;
            }
            else
            {
                if (_orientation == ScreenOrientation.Portrait || _orientation == ScreenOrientation.PortraitUpsideDown)
                    gamePlayImage.sprite = VerticalUI;
                else
                    gamePlayImage.sprite = HorizontalUI;
            }
        }
    }
}