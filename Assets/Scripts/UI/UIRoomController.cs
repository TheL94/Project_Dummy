using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.Rooms.Cells;


namespace DumbProject.UI
{
    public class UIRoomController : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerUpHandler, IDropHandler
    {
        [HideInInspector]
        public Room ActualRoom;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!eventData.dragging && ActualRoom != null)
                ActualRoom.RotateClockwise();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.dragging && ActualRoom != null)
                ActualRoom.RoomMovment.DragActions(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.dragging && ActualRoom != null)
            {
                if (ActualRoom.RoomMovment.DropActions(eventData))
                {
                    GameManager.I.RoomGenerator.ReleaseRoomSpawn(ActualRoom);
                    GameManager.I.RoomGenerator.CreateNewRoom();
                }
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.GetComponent<IventoryItem>() != null)
            {
                Cell tempCell = ActualRoom.ChooseFreeCell();
                if (tempCell != null)
                {
                    PlaceIteminRoom(eventData.pointerDrag.GetComponent<IventoryItem>().ItemToInstantiate, tempCell);
                    eventData.pointerDrag.GetComponent<IventoryItem>().DestroyObj();
                }
                else 
                    eventData.pointerDrag.GetComponent<IventoryItem>().PlaceInStartPosition();
            }
        }

        /// <summary>
        /// Piazza l'elemento passato nella posizione della cella che gli viene passata
        /// </summary>
        void PlaceIteminRoom(GameObject _object, Cell _cell)
        {
            Instantiate(_object, new Vector3(_cell.transform.position.x, _cell.transform.position.y + 2, _cell.transform.position.z), Quaternion.identity, _cell.transform);
            _cell.IsFree = false;
        }
    }
}