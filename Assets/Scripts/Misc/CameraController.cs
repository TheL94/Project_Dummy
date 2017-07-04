﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DumbProject
{
    public class CameraController : MonoBehaviour
    {
        public float MovementSpeed = 10f;
        /// <summary>
        /// Muove la transform della camera
        /// </summary>
        /// <param name="_target">La direzione in cui si deve muovere</param>
        public void MoveCamera(Vector2 _target)
        {
            transform.position = transform.position + new Vector3(_target.x, 0, _target.y) * MovementSpeed * Time.deltaTime;
        }

        /// <summary>
        /// Ruota la camera
        /// </summary>
        public void RotateCamera()
        {

        }
    }
}