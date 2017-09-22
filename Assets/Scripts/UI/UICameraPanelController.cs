using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class UICameraPanelController : MonoBehaviour
    {
        public Vector2 AnchorMin { get { return (transform as RectTransform).anchorMin; } }
        public Vector2 AnchorMax { get { return (transform as RectTransform).anchorMax; } }

        public bool CheckIfInputIsForCamera(Vector2 _position)
        {
            Vector2 deviceResolution = GameManager.I.UIMng.CurrentResolution;
            Vector2 cameraPanelPercentage;
            Vector2 cameraPanelResolution;
            if(GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.Portrait || GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.PortraitUpsideDown)
            {
                cameraPanelPercentage = new Vector2(AnchorMin.y, AnchorMin.x);
                cameraPanelResolution = new Vector2(deviceResolution.y * cameraPanelPercentage.y, deviceResolution.x * cameraPanelPercentage.x);

                if (_position.y > cameraPanelResolution.y && _position.x > cameraPanelResolution.x)
                    return true;
                else
                    return false;
            }
            else
            {
                cameraPanelPercentage = new Vector2(AnchorMax.x - AnchorMin.x, AnchorMax.y - AnchorMin.y);
                cameraPanelResolution = new Vector2(deviceResolution.x * cameraPanelPercentage.x, deviceResolution.y * cameraPanelPercentage.y);

                if (_position.x < cameraPanelResolution.x && _position.y < cameraPanelResolution.y)
                    return true;
                else
                    return false;
            }
        }
    }
}
