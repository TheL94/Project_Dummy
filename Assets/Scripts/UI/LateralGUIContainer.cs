using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.UI
{
    public class LateralGUIContainer : MonoBehaviour
    {
        public InventoryPanelController InventoryController;
        public RoomPanelContainer RoomPreviewController;
        UIGamePlayController controller;

        public void Init(UIGamePlayController _controller)
        {
            controller = _controller;
            RoomPreviewController.Setup();
        }
    }
}