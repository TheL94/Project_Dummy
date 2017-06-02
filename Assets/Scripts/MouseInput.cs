using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInput : MonoBehaviour {

    float doubleClickStart = 0;

    Room room;

    private void Start()
    {
        room = GetComponent<Room>();
    }

    private void OnMouseDown()
    {
        room.startPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        if (!room.InPosition)
        {
            MousePositionToGridPosition();
        }
    }

    void OnMouseUp()
    {
        if (!room.InPosition)
        {
            if ((Time.time - doubleClickStart) <= 0.4f)
            {
                DoubleClickActions();
                doubleClickStart = -1;
            }
            else
            {
                doubleClickStart = Time.time;
                DropAction();
            } 
        }
    }

    void DoubleClickActions()
    {
        room.Rotate();
    }

    void MousePositionToGridPosition()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        GridNode node = GameManager.I.GridCtrl.GetSpecificGridNode(objPosition);

        if (node != null)
            transform.position = node.WorldPosition;
        else
            transform.position = objPosition;
    }

    void DropAction()
    {
        ///TODO: aggiungere un controllo se è può essere posizionata. effettuando un doppio clic, senza attivare la rotate, 
        /// entra automaticamente nella funzione dropAction, spostando la room nella UI
        
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        GridNode node = GameManager.I.GridCtrl.GetSpecificGridNode(objPosition);

        if (node != null & node.RelativeCell == null)
        {
            if (GameManager.I.GridCtrl.CheckAdjacentNodesRelativeCell(node))
            {
                transform.position = node.WorldPosition;
                node.RelativeCell = GetComponent<Cell>();
                room.InPosition = true;
                return;
            }
        }
        transform.position = room.startPosition;   
    }
}
