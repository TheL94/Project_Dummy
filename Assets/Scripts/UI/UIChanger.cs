using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class UIChanger : MonoBehaviour
    {
        [HideInInspector]
        public Image ImageToChange;
        public Sprite VerticalImage;
        public Sprite HorizontalImage;


        public virtual void SetUIOrientation(ScreenOrientation _orientation)
        {
            if (ImageToChange == null)
                ImageToChange = GetComponent<Image>();

            if (GameManager.I.DeviceEnvironment == DeviceType.Desktop)
            {
                if (GameManager.I.UIMng.ForceVerticalUI)
                    ImageToChange.sprite = VerticalImage;
                else
                    ImageToChange.sprite = HorizontalImage;
            }
            else
            {
                if (_orientation == ScreenOrientation.Portrait || _orientation == ScreenOrientation.PortraitUpsideDown)
                    ImageToChange.sprite = VerticalImage;
                else
                    ImageToChange.sprite = HorizontalImage;
            }
        }
    }
}