using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.UI
{
    public class RoomPanelController : MonoBehaviour
    {
        public List<UISpawn> UISpawns = new List<UISpawn>();

        public void Setup()
        {
            foreach (UIRoomController UICtrl in GetComponentsInChildren<UIRoomController>())
                UISpawns.Add(new UISpawn(UICtrl, true));
        }

        public UIRoomController GetFirstUICtrlAvailable()
        {
            foreach (UISpawn uiSpawn in UISpawns)
                if (uiSpawn.IsAvailable)
                {
                    uiSpawn.IsAvailable = false;
                    return uiSpawn.UICtrl;
                }
            return null;
        }
    }

    public class UISpawn
    {
        public UIRoomController UICtrl;
        public bool IsAvailable;

        public UISpawn(UIRoomController _uiCtrl, bool _isAvailable)
        {
            UICtrl = _uiCtrl;
            IsAvailable = _isAvailable;
        }
    }
}