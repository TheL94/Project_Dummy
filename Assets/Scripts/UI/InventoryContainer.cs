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
            foreach (RectTransform item in GetComponentsInChildren<RectTransform>())
            {
                if(item.tag == "PausePanel")
                {
                    GamePlayElements.PausePanel = item.gameObject;
                    item.gameObject.SetActive(false);
                    break;
                }
            }
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
        public GameObject PausePanel;
    }
}