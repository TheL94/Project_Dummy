using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class CreditsMenuController : MonoBehaviour
    {
        UIMenuController menuController;
        public Button ExitButton;

        Image creditMenuImage;
        public Sprite VerticalUI;
        public Sprite HorizontalUI;

        public void Init(UIMenuController _controller, ScreenOrientation _orientation)
        {
            menuController = _controller;
            creditMenuImage = GetComponent<Image>();
            SwitchCreditMenuImage(_orientation);
        }

        private void OnEnable()
        {
            ExitButton.onClick.AddListener(() => { menuController.ShowMainMenu(); });
        }

        private void OnDisable()
        {
            ExitButton.onClick.RemoveAllListeners();
        }

        void SwitchCreditMenuImage(ScreenOrientation _orientation)
        {
            if (GameManager.I.DeviceEnvironment == DeviceType.Desktop)
            {
                if (GameManager.I.UIMng.ForceVerticalUI)
                    creditMenuImage.sprite = VerticalUI;
                else
                    creditMenuImage.sprite = HorizontalUI;
            }
            else
            {
                if (_orientation == ScreenOrientation.Portrait || _orientation == ScreenOrientation.PortraitUpsideDown)
                    creditMenuImage.sprite = VerticalUI;
                else
                    creditMenuImage.sprite = HorizontalUI;
            }
        }
    }
}