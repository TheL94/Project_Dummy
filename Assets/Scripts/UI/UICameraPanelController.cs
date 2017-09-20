using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            Vector2 cameraPanelPercentage = new Vector2(AnchorMax.x - AnchorMin.x, AnchorMax.y - AnchorMin.y);
            Vector2 cameraPanelResolution = new Vector2(deviceResolution.x * cameraPanelPercentage.x, deviceResolution.y * cameraPanelPercentage.y);

            if (_position.x < cameraPanelResolution.x && _position.y < cameraPanelResolution.y)
                return true;
            else
                return false;
        }
    }
}
