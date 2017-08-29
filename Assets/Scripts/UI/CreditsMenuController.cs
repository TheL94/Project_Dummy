using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DumbProject.UI
{
    public class CreditsMenuController : MonoBehaviour
    {
        UIMenuController menuController;
        public Button ExitButton;

        public void Init(UIMenuController _controller)
        {
            menuController = _controller;
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