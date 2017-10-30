using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class CreditsMenuController : UIChanger
    {
        UIMenuController menuController;
        public Button ExitButton;

        

        public void Init(UIMenuController _controller)
        {
            menuController = _controller;
            ImageToChange = GetComponent<Image>();
        }

        private void OnEnable()
        {
            ExitButton.onClick.AddListener(() => { menuController.ShowMainMenu(); });
        }

        private void OnDisable()
        {
            ExitButton.onClick.RemoveAllListeners();
        }
        
    }
}