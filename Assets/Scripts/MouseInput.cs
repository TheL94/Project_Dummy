using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour {

    Vector3 startPosition;
    List<CheckRoomCollision> CheckRoomControllers = new List<CheckRoomCollision>();
    bool InPosition = false;
    float doubleClickStart = 0;

    private void Start()
    {
        foreach (CheckRoomCollision controller in GetComponentsInChildren<CheckRoomCollision>())
        {
            CheckRoomControllers.Add(controller);
        }
    }

    private void OnMouseDown()
    {
        startPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        if (!InPosition)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100);
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition;
        }
    }

    void OnMouseUp()
    {
        if ((Time.time - doubleClickStart) <= 0.3f)
        {
            DoubleClickActions();
            doubleClickStart = -1;
        }
        else
        {
            doubleClickStart = Time.time;
            SingleClickActions();
        }
    }

    void DoubleClickActions()
    {
        Debug.Log("Double Clicked!");
        // TODO : chiammare la funzione di rotazione della stanza su cui stai cliccando
    }

    void SingleClickActions()
    {
        if (!InPosition)
        {
            foreach (CheckRoomCollision controller in CheckRoomControllers)
            {
                if (controller.CanAttach == true)
                {
                    Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100);
                    Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    transform.position = objPosition;
                    InPosition = true;
                }
                else
                    transform.position = startPosition;
            }
        }
    }
}
