using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.UI
{
    public class RoomPanelController : MonoBehaviour
    {
        List<UIRoomController> _uiSpawns = new List<UIRoomController>();

        public List<UIRoomController> UISpawns { get { return _uiSpawns; } }

        public void Setup()
        {
            foreach (UIRoomController uiCtrl in GetComponentsInChildren<UIRoomController>())
                _uiSpawns.Add(uiCtrl);
        }
    }
}