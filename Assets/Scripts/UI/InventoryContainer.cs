using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.UI
{
    public class InventoryContainer : MonoBehaviour
    {
        GamePlayUIElements GamePlayElements;

        public void Init()
        {
            GamePlayElements = new GamePlayUIElements() { InventoryController = GetComponentInChildren<InventoryController>(), RoomPreviewController = GetComponentInChildren<RoomPanelController>() };
            GamePlayElements.RoomPreviewController.Setup();
        }

        public GamePlayUIElements GetGamePlayElements()
        {
            return GamePlayElements;
        }
    }

    public struct GamePlayUIElements
    {
        public InventoryController InventoryController;
        public RoomPanelController RoomPreviewController;
    }
}