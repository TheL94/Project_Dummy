using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Rooms;
using Framework.Grid;
using Framework.UI;

public class GameManager : MonoBehaviour {

    public static GameManager I;

    public RoomGenertor RoomGenertorPrefab;
    public GridController GridControllerPrefab;
    public UIManager UIManagerPrefab;

    [HideInInspector]
    public RoomGenertor RoomGenertor;
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
        RoomGenertor = Instantiate(RoomGenertorPrefab);

        CreateRoomPreview();
        GridCtrl.Setup();
        UIMng.CreateCanvasGame();
        RoomGenertor.Setup();
    }

    public void CreateRoomPreview()
    {
        RoomPreviewCtrl = Instantiate(Resources.Load("Prefabs/RoomUIPreviews/RoomPreviewsContainer") as GameObject, transform).GetComponentInChildren<RoomPreviewController>();
        RoomPreviewCtrl.Init();
    }
}
