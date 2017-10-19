using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;


namespace DumbProject.UI
{
    public class MainMenuController : MonoBehaviour
    {
        UIMenuController menuController;

        Image menuImage;
        public Sprite VerticalUI;
        public Sprite HorizontalUI;


        public Button PlayButton;
        public Button CreditsButton;
        public Button ExitButton;

        public void Init(UIMenuController _controller, ScreenOrientation _orientation)
        {
            menuController = _controller;
            menuImage = GetComponent<Image>();
            SwitchMainMenuImage(_orientation);
        }

        private void OnEnable()
        {
            PlayButton.onClick.AddListener(() => { menuController.PlayGame(); });          
            CreditsButton.onClick.AddListener(() => { menuController.ShowCredits(); });
            ExitButton.onClick.AddListener(() => { menuController.QuitApplication(); });
        }

        private void OnDisable()
        {
            PlayButton.onClick.RemoveAllListeners();
            CreditsButton.onClick.RemoveAllListeners();
            ExitButton.onClick.RemoveAllListeners();
        }

        void SwitchMainMenuImage(ScreenOrientation _orientation)
        {
            if (GameManager.I.DeviceEnvironment == DeviceType.Desktop)
            {
                if (GameManager.I.UIMng.ForceVerticalUI)
                    menuImage.sprite = VerticalUI;
                else
                    menuImage.sprite = HorizontalUI;
            }
            else
            {
                if (_orientation == ScreenOrientation.Portrait || _orientation == ScreenOrientation.PortraitUpsideDown)
                    menuImage.sprite = VerticalUI;
                else
                    menuImage.sprite = HorizontalUI;
            }
        }

    }
}