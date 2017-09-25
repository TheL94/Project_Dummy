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

            if(GameManager.I.DeviceEnvironment == DeviceType.Desktop)
            {
                if (GameManager.I.UIMng.ForceVerticalUI)
                    return CheckPortraitPosition(_position, deviceResolution);
                else
                    return CheckLandscapePosition(_position, deviceResolution);
            }
            else
            {
                if (GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.Portrait || GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.PortraitUpsideDown)
                    return CheckPortraitPosition(_position, deviceResolution);
                else
                    return CheckLandscapePosition(_position, deviceResolution);
            }

        }

        bool CheckPortraitPosition(Vector2 _position, Vector2 _deviceResolution)
        {
            Vector2 cameraPanelPercentage = new Vector2(AnchorMin.y, AnchorMin.x);
            Vector2 cameraPanelResolution = new Vector2(_deviceResolution.y * cameraPanelPercentage.y, _deviceResolution.x * cameraPanelPercentage.x);

            if (_position.y > cameraPanelResolution.y && _position.x > cameraPanelResolution.x)
                return true;
            else
                return false;
        }

        bool CheckLandscapePosition(Vector2 _position, Vector2 _deviceResolution)
        {
            Vector2 cameraPanelPercentage = new Vector2(AnchorMax.x - AnchorMin.x, AnchorMax.y - AnchorMin.y);
            Vector2 cameraPanelResolution = new Vector2(_deviceResolution.x * cameraPanelPercentage.x, _deviceResolution.y * cameraPanelPercentage.y);

            Debug.Log(_position + " / " + cameraPanelResolution);

            if (_position.x < cameraPanelResolution.x && _position.y < cameraPanelResolution.y)
                return true;
            else
                return false;
        }
    }
}
