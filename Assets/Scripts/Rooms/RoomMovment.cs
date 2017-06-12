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

        /// <summary>
        /// Azioni eseguite durante il drag della stanza
        /// </summary>
        /// <param name="_eventData"></param>
        public void DragActions(PointerEventData _eventData)
        {
            Vector3 mousePosition = new Vector3(Camera.main.ScreenToWorldPoint(_eventData.position).x, Camera.main.nearClipPlane, Camera.main.ScreenToWorldPoint(_eventData.position).z);
            GridNode node = GameManager.I.MainGridCtrl.GetSpecificGridNode(mousePosition);

            if (node != null)
            {
                transform.DOMove(node.WorldPosition, 0.05f);
                foreach (Cell cell in room.CellsInRoom)
                {
                    if (node == null || (node != null && node.RelativeCell != null))
                        cell.ShowInvalidPosition(true);
                    else
                        cell.ShowInvalidPosition(false);
                }
            }
            else
                transform.position = mousePosition;
        }


        /// <summary>
        /// Azioni eseguite al drop della stanza
        /// </summary>
        /// <param name="_eventData"></param>
        /// <returns></returns>
        public bool DropActions(PointerEventData _eventData)
        {
            if (room.CheckPosition())
            {
                room.PlaceAction();
                return true;
            }
            else
            {
                room.ResetPositionToInitialPosition();
                return false;
            }           
        }
    }
}