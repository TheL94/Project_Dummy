using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [HideInInspector]
    public RoomPreviewController roomPreviewController;

    public void CreateCanvasGame()
    {
        roomPreviewController = Instantiate(Resources.Load("Prefabs/UI/CanvasGame") as GameObject, transform).GetComponentInChildren<RoomPreviewController>();
        roomPreviewController.Init();
    }

    public void DestroyCanvasGame()
    {
        Destroy(roomPreviewController.gameObject);
    }
}
