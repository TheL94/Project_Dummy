using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class CameraBackground : MonoBehaviour, IUIChanger
    {
        Image backGround;
        public Sprite VerticalBackground;
        public Sprite HorizontalBackground;

        private void Start()
        {
            backGround = GetComponent<Image>();
        }

        public void SetUIOrientation(ScreenOrientation _orientation)
        {
            if(backGround == null)
                backGround = GetComponent<Image>();

            if (GameManager.I.DeviceEnvironment == DeviceType.Desktop)
            {
                if (GameManager.I.UIMng.ForceVerticalUI)
                    backGround.sprite = VerticalBackground;
                else
                    backGround.sprite = HorizontalBackground;
            }
            else
            {
                if (_orientation == ScreenOrientation.Portrait || _orientation == ScreenOrientation.PortraitUpsideDown)
                    backGround.sprite = VerticalBackground;
                else
                    backGround.sprite = HorizontalBackground;
            }
        }
    }
}