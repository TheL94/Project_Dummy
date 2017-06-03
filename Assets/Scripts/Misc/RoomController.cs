using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {

    float doubleClickStart = 0;
    public Room Room;
    float timeToHold = 0.15f;
    Vector3 _roomStartPosition;
    public Vector3 RoomStartPosition { get
        { return _roomStartPosition; }
        set {
            if (_roomStartPosition == new Vector3(0,0,0))
            {
                _roomStartPosition = value; 
            }
        }
    }

    private void OnMouseDown()
    {
        RoomStartPosition = Room.transform.position;
        
    }


    private void OnMouseDrag()
    {
        timeToHold -= Time.deltaTime;
        if(timeToHold <= 0)
        {
            Room.GetComponent<MouseInput>().MousePositionToGridPosition();
        }
    }

    private void OnMouseUp()
    {
        if ((Time.time - doubleClickStart) <= 0.2f)
        {
            RotateRoomAction();
            doubleClickStart = -1;
        }
        else
        {
            doubleClickStart = Time.time;
            DropAction();
        }
        timeToHold = 0.15f;
    }

    void DropAction()
    {
        //Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100);
        //Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Room.transform.position = RoomStartPosition;
    }
    public void RotateRoomAction()
    {
        Room.Rotate();
    }

}
