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
    public RoomPreviewController RoomPreviewControllerPrefab;

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
        RoomPreviewCtrl = Instantiate(RoomPreviewControllerPrefab);
        RoomGenerator = Instantiate(RoomGenertorPrefab);


        GridCtrl.Setup();
        UIMng.CreateCanvasGame();
        RoomPreviewCtrl.Setup();
        RoomGenerator.Setup();
    }
}
