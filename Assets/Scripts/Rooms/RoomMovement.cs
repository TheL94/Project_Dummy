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
        Tweener snap;
        GridNode _closerNode;
        public GridNode closerNode
        {
            get { return _closerNode; }
            set {
                if(value != null)
                {
                    SafeSnap(value.WorldPosition);
                    CheckCollisions();
                }
                _closerNode = value;
            }
        }
        bool _movingInitialPosition;
        public bool MovingToInitialPosition
        {
            get { return _movingInitialPosition; }
            set {
                _movingInitialPosition = value;
                if (value)
                    SafeSnap(room.InitialPosition);
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


            closerNode = GameManager.I.MainGridCtrl.GetSpecificGridNode(mousePosition);
            if (closerNode != null)
            {
                //room.transform.position = closerNode.WorldPosition;
                foreach (Cell cell in room.CellsInRoom)
                {
                    // TODO : 
                    //if (closerNode == null || (closerNode != null && closerNode.RelativeCell != null))
                    //    cell.ShowInvalidPosition(true);
                    //else
                    //    cell.ShowInvalidPosition(false);
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
            if (GameManager.I.DungeonMng.DropCtrl.CheckRoomValidPosition(room))
            {
                room.PlaceAction();
                return true;
            }
            else
            {
                MovingToInitialPosition = true;
                return false;
            }
        }

        void SafeSnap(Vector3 _nodePosition)
        {
            if (_nodePosition == null || closerNode == null)
                return;

            if (_nodePosition != closerNode.WorldPosition)
            {
                if (snap != null)
                    snap.Complete();
                if (!MovingToInitialPosition)
                    snap = room.transform.DOMove(_nodePosition, .05f);
                else
                    snap = room.transform.DOMove(_nodePosition, .1f).OnComplete(() => { MovingToInitialPosition = false; });
            }
        }

        void CheckCollisions()
        {
            foreach (Cell cell in room.CellsInRoom)
            {
                foreach (Edge edge in cell.GetEdgesList())
                {
                    edge.CheckCollisionWithOtherEdges(GameManager.I.MainGridCtrl);
                }
            }
        }
    }
}