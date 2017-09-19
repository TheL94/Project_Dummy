using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DumbProject.Grid;
using DG.Tweening;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    public class RoomMovement : MonoBehaviour
    {
        Room room;

        Vector3 _roomInitialPosition;
        public Vector3 RoomInitialPosition
        {
            get { return _roomInitialPosition; }
            private set { _roomInitialPosition = value; }
        }

        Tweener rotationTween;
        bool canRotate = true;

        Tweener snap;
        GridNode _closerNode;
        Vector3 mousePosition;
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
                    SafeSnap(RoomInitialPosition);
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
            RoomInitialPosition = room.transform.position;
        }

        /// <summary>
        /// Azioni eseguite durante il drag della stanza
        /// </summary>
        /// <param name="_eventData"></param>
        public void DragActions(PointerEventData _eventData)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(_eventData.position);
            //------------Sistem con Raycast--------
            mouseProjection = Camera.main.ScreenPointToRay(_eventData.position);
            float distance;
            if (gridLevel.Raycast(mouseProjection, out distance))
                mousePosition = mouseProjection.GetPoint(distance);
            //---------------------------------------

            closerNode = GameManager.I.MainGridCtrl.GetSpecificGridNode(mousePosition);
            if (closerNode != null)
            {
                room.transform.position = closerNode.WorldPosition;
                if (GameManager.I.DungeonMng.DropCtrl.CheckRoomValidPosition(room))
                    room.HighlightRoomWalls(true, Color.green);
                else
                    room.HighlightRoomWalls(true, Color.red);
            }
            else
            {
                transform.position = mousePosition;
                room.HighlightRoomWalls(true, Color.red);
            }
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
                snap.Complete(true);
                room.PlaceAction();
                room.HighlightRoomWalls(false, Color.black);
                return true;
            }
            else
            {
                MovingToInitialPosition = true;
                room.HighlightRoomWalls(false, Color.black);
                return false;
            }
        }

        /// <summary>
        /// funzione che fa ruotare la stanza attorno al suo centro in senso orario sulla griglia 
        /// </summary>
        public void RotateClockwise()
        {
            if (canRotate)
            {
                canRotate = false;
                rotationTween = room.transform.DORotate(transform.up * 90f, 0.5f, RotateMode.LocalAxisAdd).OnComplete(() =>
                {
                    canRotate = true;
                });
            }
        }

        void SafeSnap(Vector3 _nodePosition)
        {
            if (_nodePosition == null && closerNode == null)
                return;

            else if ((_nodePosition != null && closerNode == null) || (_nodePosition != closerNode.WorldPosition))
            {
                if (snap != null)
                    snap.Complete();
                if (!MovingToInitialPosition)
                    snap = room.transform.DOMove(_nodePosition, 0.3f);
                else
                    snap = room.transform.DOMove(_nodePosition, 0).OnComplete(() => { MovingToInitialPosition = false; });
            }
        }

        void CheckCollisions()
        {
            List<Edge> cellEdges = null;
            foreach (Cell cell in room.CellsInRoom)
            {
                cellEdges = new List<Edge>();
                cellEdges.AddRange(cell.Edges);
                cellEdges.AddRange(cell.Doors.ConvertAll(d => d as Edge));

                foreach (Edge edge in cellEdges)
                    edge.CheckCollisionWithOtherEdges(GameManager.I.MainGridCtrl);
            }
        }
    } 
}