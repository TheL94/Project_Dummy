using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DumbProject.Grid;

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
            // TODO : evitare di usare il find (non saprei come fare per ora)
            List<Angle> anglesInFrontCell = FindObjectsOfType<Angle>().ToList();
            anglesInFrontCell.Remove(this);
            foreach (Angle angleInFront in anglesInFrontCell)
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