using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class RoomPanelContainer : UIChanger
    {
        public List<UIRoomController> UISpawns { get { return _uiSpawns; } }

        List<UIRoomController> _uiSpawns = new List<UIRoomController>();

        public void Setup()
        {
            ImageToChange = GetComponent<Image>();
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
    }
}