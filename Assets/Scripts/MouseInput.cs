using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour {

    Vector3 startPosition;
    bool InPosition = false;
    float doubleClickStart = 0;

    Room room;

    private void Start()
    {
        room = GetComponent<Room>();
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
        if ((Time.time - doubleClickStart) <= 0.4f)
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
        room.Rotate();
    }

    void SingleClickActions()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        GridNode node = GameManager.I.GridCtrl.GetSpecificGridNode(objPosition);

        if (node != null)
        {
            if(GameManager.I.GridCtrl.CheckAdjacentNodesRelativeCell(node))
            {
                transform.position = node.WorldPosition;
                node.RelativeCell = GetComponent<Cell>();
                InPosition = true;
                return;
            }
        }
        transform.position = startPosition;
    }
}
