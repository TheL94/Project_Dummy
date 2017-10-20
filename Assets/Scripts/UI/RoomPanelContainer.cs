using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class RoomPanelContainer : MonoBehaviour, IUIChanger
    {
        public Sprite VerticalUI;
        public Sprite HorizontalUI;

        public List<UIRoomController> UISpawns { get { return _uiSpawns; } }

        Image gamePlayImage;
        List<UIRoomController> _uiSpawns = new List<UIRoomController>();

        public void Setup(ScreenOrientation _orientation)
        {
            gamePlayImage = GetComponent<Image>();
            SetUIOrientation(_orientation);
            foreach (UIRoomController uiCtrl in GetComponentsInChildren<UIRoomController>())
            {
                if(!UISpawns.Contains(uiCtrl))
                    _uiSpawns.Add(uiCtrl);
            }
        }

        public bool CheckIfInputIsForRoomPreviews(Vector2 _position, out UIRoomController _UIRoomController)
        {
            foreach (UIRoomController roomPreview in UISpawns)
            {
                if (roomPreview.CheckIfInputIsForRoomPreview(_position))
                {
                    _UIRoomController = roomPreview;
                    return true;
                }
            }
            _UIRoomController = null;
            return false;
        }

        public void SetUIOrientation(ScreenOrientation _orientation)
        {
            if (GameManager.I.DeviceEnvironment == DeviceType.Desktop)
            {
                if (GameManager.I.UIMng.ForceVerticalUI)
                    gamePlayImage.sprite = VerticalUI;
                else
                    gamePlayImage.sprite = HorizontalUI;
            }
            else
            {
                if (_orientation == ScreenOrientation.Portrait || _orientation == ScreenOrientation.PortraitUpsideDown)
                    gamePlayImage.sprite = VerticalUI;
                else
                    gamePlayImage.sprite = HorizontalUI;
            }
        }
    }
}