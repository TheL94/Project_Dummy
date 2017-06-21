using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;

namespace DumbProject.Rooms.Cells.Doors
{
    public class Door : MonoBehaviour
    {
        Cell _relativeCell;
        public Cell RelativeCell
        {
            get { return _relativeCell; }
            private set { _relativeCell = value; }
        }

        public void Setup(Cell _cell)
        {
            RelativeCell = _cell;     
        }

        public bool CheckCollisionWithOtherDoor(Cell _adjacentCell)
        {
            bool isInValidPosition = false;
            if (_adjacentCell != null)
            {
                foreach (Door door in _adjacentCell.RelativeRoom.DoorsInRoom)
                {
                    if (Vector3.Distance(door.transform.position, transform.position) <= RelativeCell.RelativeRoom.Data.PenetrationOffset)
                    {
                        isInValidPosition = true;
                    }
                }
            }
            return isInValidPosition;
        }
    }
}

