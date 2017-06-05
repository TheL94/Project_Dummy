using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomPreviewController : MonoBehaviour {

    public List<Transform> RoomSpawns = new List<Transform>();
    public List<UIRoomController> UIRoomControllers = new List<UIRoomController>();

    public void Init()
    {
        UIRoomControllers = GetComponentsInChildren<UIRoomController>().ToList();
    }
}
