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

        /// <summary>
        /// Funzione che controlla che la porta stia compenenetrando un'altra porta
        /// </summary>
        /// <param name="_adjacentCell"></param>
        /// <returns></returns>
        public bool CheckCollisionWithOtherDoors(Cell _adjacentCell)
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

        /// <summary>
        /// Funzione che controlla che la porta non stia compenenetrando un muro
        /// </summary>
        /// <param name="_roomToCheck"></param>
        /// <returns></returns>
        public bool CheckCollisionWithOtherWalls(Room _roomToCheck)
        {
            bool isInvalidPosition = true;
            foreach (GameObject wall in _roomToCheck.GetListOfWalls())
            {
                if (Vector3.Distance(wall.transform.position, transform.position) <= RelativeCell.RelativeRoom.Data.PenetrationOffset)
                {
                    isInvalidPosition = false;
                }
            }
            return isInvalidPosition;
        }
    }
}

