using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DumbProject
{
    public class CameraController : MonoBehaviour
    {
        float MovementSpeed = 10f;
        /// <summary>
        /// Muove la transform della camera
        /// </summary>
        /// <param name="_target">La direzione in cui si deve muovere</param>
        public void MoveCamera(Vector3 _target)
        {
            transform.position = transform.position + _target * MovementSpeed * Time.deltaTime;
            //transform.Translate(new Vector3(_target.x, _target.y, 0 ).normalized * MovementSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Ruota la camera
        /// </summary>
        public void RotateCamera(Quaternion _rotation)
        {
            transform.rotation = _rotation;
        }

        public void ZoomTheCamera(float _direction)
        {
            if (transform.position.y <= 150 && transform.position.y >= 100)
            {
                transform.Translate(Vector3.forward * _direction * Time.deltaTime); 
            }
        }
    }
}