using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Rooms.Cells;
using DumbProject.Rooms.Cells.Doors;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    public class DropController : MonoBehaviour
    {
        Room room;

        public void Init(Room _room)
        {
            room = _room;
        }

        /// <summary>
        /// Funzione che controlla che la posizione di ogni cella della stanza sia valida
        /// </summary>
        public bool CheckPosition()
        {
            bool IsValidPosition = false;
            foreach (Cell cell in room.CellsInRoom)
            {
                GridNode node;
                if (!cell.CheckValidPosition(GameManager.I.MainGridCtrl, out node))
                    return false;

                foreach (GridNode adjacentNode in node.AdjacentNodes)
                {
                    if (adjacentNode.RelativeCell != null)
                    {
                        foreach (Door door in room.DoorsInRoom)
                        {
                            if (door.CheckCollisionWithOtherDoors(adjacentNode.RelativeCell))
                                IsValidPosition = true;
                        }
                    }
                }
            }
            return IsValidPosition;
        }
    }
}