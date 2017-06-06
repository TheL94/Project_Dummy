using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DumbProject.UI
{
    public class RoomPanelController : MonoBehaviour
    {
        public List<UIRoomController> UIRoomControllers = new List<UIRoomController>();

        public void Init()
        {
            UIRoomControllers = GetComponentsInChildren<UIRoomController>().ToList();
        }
    }
}