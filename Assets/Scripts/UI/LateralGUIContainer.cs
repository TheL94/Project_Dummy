using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.UI
{
    public class LateralGUIContainer : MonoBehaviour
    {
        public InventoryPanelController InventoryController;
        public RoomPanelContainer RoomPreviewController;

        public void Init()
        {
            RoomPreviewController.Setup();
        }
    }
}