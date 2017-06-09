using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DumbProject.Grid;
using DumbProject.Rooms.Cells;
using DG.Tweening;

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
            Vector3 mousePosition = new Vector3(Camera.main.ScreenToWorldPoint(_eventData.position).x, Camera.main.nearClipPlane, Camera.main.ScreenToWorldPoint(_eventData.position).z);
            GridNode node = GameManager.I.MainGridCtrl.GetSpecificGridNode(mousePosition);

            if (node != null)
            {
                transform.DOMove(node.WorldPosition, 0.1f);
                foreach (Cell cell in room.CellsInRoom)
                {
                    node = GameManager.I.MainGridCtrl.GetSpecificGridNode(cell.transform.position);
                    if (node == null || (node != null && node.RelativeCell != null))
                        cell.ShowInvalidPosition(true);
                    else
                        cell.ShowInvalidPosition(false);

                }
            }
            else
                transform.position = mousePosition;
        }

        public bool DropActions(PointerEventData _eventData)
        {
            GridNode nodeInGrid;
            foreach (Cell cellInRoom in room.CellsInRoom)
            {
                nodeInGrid = GameManager.I.MainGridCtrl.GetSpecificGridNode(cellInRoom.transform.position);
                if (nodeInGrid != null && nodeInGrid.RelativeCell == null)
                {
                    if (GameManager.I.MainGridCtrl.CheckAdjacentNodesRelativeCell(nodeInGrid))
                    {
                        GridNode node;
                        foreach (Cell cell in room.CellsInRoom)
                        {
                            node = GameManager.I.MainGridCtrl.GetSpecificGridNode(cell.transform.position);
                            if(node == null || (node != null && node.RelativeCell != null))
                            {
                                room.transform.position = room.StartPosition;
                                foreach (Cell c in room.CellsInRoom)
                                    c.ShowInvalidPosition(false);
                                return false;  
                            }
                            else
                                cell.PlaceCellInMainGrid(node);

                        }
                        room.transform.parent = null;
                        return true;
                    }
                }
            }
            room.transform.position = room.StartPosition;
            return false;
        }
    }
}