using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;


namespace DumbProject.UI
{
    public class MainMenuController : UIChanger
    {
        UIMenuController menuController;

        public Button PlayButton;
        public Button CreditsButton;
        public Button ExitButton;

        public void Init(UIMenuController _controller)
        {
            menuController = _controller;
            ImageToChange = GetComponent<Image>();
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
    }
}