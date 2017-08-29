using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    public class Angle : CellElement
    {
        [HideInInspector]
        public Angle CollidingAngle;

        GameObject lightingObject;

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

        /// <summary>
        /// Funzione che ritorna la posizione che sta di fronte a se stesso
        /// </summary>
        /// <returns></returns>
        public Vector3 GetOppositeOfRelativeCellPosition()
        {
            return GetFrontPosition(RelativeCell.transform.position);
        }

        public Vector3 GetFrontPosition(Vector3 _position)
        {
            return (transform.position * 2) - _position;
        }

        public virtual void SetLightingObject(GameObject _lightingObj, Vector3 _position)
        {
            lightingObject = _lightingObj;
            lightingObject.transform.position = _position;
            lightingObject.transform.parent = transform;
        }

        public override void DisableGraphicElement()
        {
            base.DisableGraphicElement();

            if (lightingObject != null)
            {
                lightingObject.SetActive(false);
                lightingObject = null;
                GameManager.I.PoolMng.UpdatePools();
            }
        }

        /// <summary>
        /// Funzione che disabilita l'oggetto
        /// </summary>
        /// <param name="_avoidDestruction"></param>
        public override void DisableAndDestroyObject(bool _avoidDestruction = false)
        {
            RelativeCell.Angles.Remove(this);
            base.DisableAndDestroyObject(_avoidDestruction);
        }
    }
}