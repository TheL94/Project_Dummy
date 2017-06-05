using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager I;

    public RoomManager RoomManagerPrefab;
    public GridController GridControllerPrefab;
    public UIManager UIManagerPrefab;

    [HideInInspector]
    public RoomManager RoomMng;
    [HideInInspector]
    public GridController GridCtrl;
    [HideInInspector]
    public UIManager UIMng;
    [HideInInspector]
    public RoomPreviewController RoomPreviewCtrl;

    private void Awake()
    {
        //Singleton paradigm
        if (I == null)
            I = this;
        else
            DestroyImmediate(gameObject);
    }

    private void Start()
    {
        GridCtrl = Instantiate(GridControllerPrefab);
        UIMng = Instantiate(UIManagerPrefab);
        RoomMng = Instantiate(RoomManagerPrefab);

        CreateRoomPreview();
        GridCtrl.Setup();
        UIMng.CreateCanvasGame();
        RoomMng.Setup();
    }

    public void CreateRoomPreview()
    {
        RoomPreviewCtrl = Instantiate(Resources.Load("Prefabs/RoomUIPreviews/RoomPreviewsContainer") as GameObject, transform).GetComponentInChildren<RoomPreviewController>();
        RoomPreviewCtrl.Init();
    }
}
