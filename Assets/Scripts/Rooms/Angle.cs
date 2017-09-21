using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    public class Angle : CellElement
    {
        [HideInInspector]
        public Angle CollidingAngle;

        /// <summary>
        /// Funzione che controlla la collisione con altri edge
        /// </summary>
        public void CheckCollisionWithOtherAngle()
        {
            List<Room> adjacentRooms = GameManager.I.DungeonMng.GetAdjacentRoomsByDoors(RelativeCell.RelativeRoom);
            List<Angle> adjacentAngles = new List<Angle>();

            foreach (Room room in adjacentRooms)
                adjacentAngles.AddRange(room.GetAngles());

            foreach (Angle angleInFront in adjacentAngles)
            {
                if (Vector3.Distance(angleInFront.transform.position, transform.position) <= RelativeCell.RelativeRoom.Data.PenetrationOffset)
                {
                    CollidingAngle = angleInFront;
                    return;
                }
                else
                    CollidingAngle = null;
            }
        }

        public override void DestroyObject(bool _destroyComponentOnly = false)
        {
            RelativeCell.Angles.Remove(this);
            base.DestroyObject(_destroyComponentOnly);
        }
    }
}