﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;


namespace DumbProject.UI
{
    public class MainMenuController : MonoBehaviour, IUIChanger
    {
        UIMenuController menuController;

        Image menuImage;
        public Sprite VerticalUI;
        public Sprite HorizontalUI;


        public Button PlayButton;
        public Button CreditsButton;
        public Button ExitButton;

        public void Init(UIMenuController _controller)
        {
            menuController = _controller;
            menuImage = GetComponent<Image>();
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
        
        public void SetUIOrientation(ScreenOrientation _orientation)
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