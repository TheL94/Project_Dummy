using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {

    float doubleClickStart = 0;
    public Room Room;
    public RotateRoom RotateRoom;

    Vector3 roomStartPosition;


    private void Start()
    {
        RotateRoom.Init(this);
    }

    private void OnMouseDown()
    {
        roomStartPosition = Room.transform.position;
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100);
        Vector3 ObjPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Room.GetComponent<MouseInput>().enabled = true;
        Room.transform.position = ObjPosition;
        Room.GetComponent<MouseInput>().IsSelected = true;
    }

    private void OnMouseUp()
    {

        if ((Time.time - doubleClickStart) <= 0.4f)
        {
            RotateRoomAction();
            doubleClickStart = -1;
        }
        else
        {
            //IsSelected = false;
            doubleClickStart = Time.time;
            DropAction();
        }
    }

    void DropAction()
    {
        Room.GetComponent<MouseInput>().IsSelected = false;
        //Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100);
        //Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Room.transform.position = roomStartPosition;
    }
    public void RotateRoomAction()
    {
        Room.Rotate();
    }

}
