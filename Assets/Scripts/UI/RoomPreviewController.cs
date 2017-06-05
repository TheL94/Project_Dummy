using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomPreviewController : MonoBehaviour {

    public List<Transform> RoomSpawns = new List<Transform>();
    public List<UIRoomController> UIRoomControllers = new List<UIRoomController>();
    List<GridController> GridControllers = new List<GridController>();

    public void Init()
    {
        UIRoomControllers = GetComponentsInChildren<UIRoomController>().ToList();
        GridControllers = GetComponentsInChildren<GridController>().ToList();

        InitGrids();
    }

    void InitGrids()
    {
        foreach (GridController grid in GridControllers)
            grid.Setup();
    }
}