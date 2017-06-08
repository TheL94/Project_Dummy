using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DumbProject.Grid;

namespace DumbProject.Rooms
{
    public class RoomMovment : MonoBehaviour
    {

        Room room;

        public void Init(Room _room)
        {
            room = _room;
        }

        public void DragActions(PointerEventData _eventData)
        {
            Vector3 objPosition = new Vector3(Camera.main.ScreenToWorldPoint(_eventData.position).x, Camera.main.nearClipPlane, Camera.main.ScreenToWorldPoint(_eventData.position).z);
            GridNode node = GameManager.I.MainGridCtrl.GetSpecificGridNode(objPosition);

            if (node != null)
                transform.position = node.WorldPosition;
            else
                transform.position = objPosition;
        }

        public bool DropActions(PointerEventData _eventData)
        {
            Vector3 objPosition = new Vector3(Camera.main.ScreenToWorldPoint(_eventData.position).x, Camera.main.nearClipPlane, Camera.main.ScreenToWorldPoint(_eventData.position).z);
            GridNode node = GameManager.I.MainGridCtrl.GetSpecificGridNode(objPosition);

            if (node != null && node.RelativeCell == null)
            {
                if (GameManager.I.MainGridCtrl.CheckAdjacentNodesRelativeCell(node))
                {
                    transform.position = node.WorldPosition;
                    room.transform.parent = null;
                    node.RelativeCell = room.CellsInRoom[0];
                    return true;
                }
            }

            room.transform.position = room.StartPosition;
            return false;
        }
    }
}