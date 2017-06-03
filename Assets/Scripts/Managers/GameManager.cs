using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager I;

    public RoomManager RoomManagerPrefab;
    public GridController GridControllerPrefab;

    [HideInInspector]
    public RoomManager RoomMng;
    [HideInInspector]
    public GridController GridCtrl;

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
        RoomMng = Instantiate(RoomManagerPrefab);
        GridCtrl = Instantiate(GridControllerPrefab);

        GridCtrl.Setup();
        RoomMng.Setup();

        Camera.main.transform.position = new Vector3 (RoomMng.FirstRoom.transform.position.x, Camera.main.transform.position.y, RoomMng.FirstRoom.transform.position.z);
    }
}
