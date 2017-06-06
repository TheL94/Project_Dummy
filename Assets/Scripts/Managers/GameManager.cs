using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using DumbProject.Grid;
using DumbProject.UI;
using DumbProject.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager I;

    public RoomGenerator RoomGenertorPrefab;
    public GridController GridControllerPrefab;
    public UIManager UIManagerPrefab;

    [HideInInspector]
    public RoomGenerator RoomGenerator;
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
        RoomGenerator = Instantiate(RoomGenertorPrefab);

        CreateRoomPreview();
        GridCtrl.Setup();
        UIMng.CreateCanvasGame();
        RoomGenerator.Setup();
    }

    public void CreateRoomPreview()
    {
        RoomPreviewCtrl = Instantiate(Resources.Load("Prefabs/RoomUIPreviews/RoomPreviewsContainer") as GameObject, transform).GetComponentInChildren<RoomPreviewController>();
        RoomPreviewCtrl.Init();
    }
}
