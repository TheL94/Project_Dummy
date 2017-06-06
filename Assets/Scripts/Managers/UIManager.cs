using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.UI
{
    public class UIManager : MonoBehaviour
    {

        [HideInInspector]
        public RoomPanelController roomPreviewController;

        public void CreateCanvasGame()
        {
            roomPreviewController = Instantiate(Resources.Load("Prefabs/UI/CanvasGame") as GameObject, transform).GetComponentInChildren<RoomPanelController>();
            roomPreviewController.Init();
        }

        public void DestroyCanvasGame()
        {
            Destroy(roomPreviewController.gameObject);
        }
    }
}