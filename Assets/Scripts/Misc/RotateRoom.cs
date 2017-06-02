using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRoom : MonoBehaviour {

    RoomController roomController;

	public void Init(RoomController _roomController)
    {
        roomController = _roomController;
    }

    private void OnMouseDown()
    {
        roomController.RotateRoomAction();
    }
}
