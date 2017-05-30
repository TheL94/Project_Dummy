using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjDrag : MonoBehaviour {

    Vector3 startPosition;
    List<CheckRoomCollision> CheckRoomControllers = new List<CheckRoomCollision>();
    bool InPosition = false;

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

    private void OnMouseUp()
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