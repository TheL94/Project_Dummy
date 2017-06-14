using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DumbProject.Grid;
using DumbProject.Rooms.Cells;
using DG.Tweening;
using DumbProject.Generic;


namespace DumbProject.Rooms
{
    public class RoomMovement : MonoBehaviour
    {
        Room room;
        Tweener _snap;
        Tweener snap
        {
            get { return _snap; }
            set
            {
                if (_snap != null)
                    _snap.Complete();
                _snap = value;
            }
        }
        //------------Sistema con Raycast--------
        Ray mouseProjection;
        Plane gridLevel;
        //---------------------------------------

        public void Init(Room _room)
        {
            //----------
            gridLevel = new Plane(Vector3.up, GameManager.I.MainGridCtrl.transform.position.y + GameManager.I.MainGridCtrl.GridOffsetY);
            //----------
            room = _room;
        }

        /// <summary>
        /// Azioni eseguite durante il drag della stanza
        /// </summary>
        /// <param name="_eventData"></param>
        public void DragActions(PointerEventData _eventData)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(_eventData.position);
            //------------Sistem con Raycast--------
            mouseProjection = Camera.main.ScreenPointToRay(_eventData.position);
            float distance;
            if (gridLevel.Raycast(mouseProjection, out distance))
                mousePosition = mouseProjection.GetPoint(distance);
            //---------------------------------------


            GridNode node = GameManager.I.MainGridCtrl.GetSpecificGridNode(mousePosition);
            if (node != null)
            {
                snap = room.transform.DOMove(node.WorldPosition, .05f);
                //room.transform.position = node.WorldPosition;
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
            if (snap != null && snap.IsPlaying())
                snap.Complete();
            //controlla che al drop le posizioni su cui vengono fatti i controlli "Check", siano stati assegnati con ordine e per tempo
            if (room.CheckPosition())
            {
                room.PlaceAction();
                return true;
            }
            else
            {
                room.ResetPositionToInitialPosition(snap);
                return false;
            }
        }
    }
}