using System;
using System.Collections;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject
{
    public class CameraMovement : MonoBehaviour
    {
        public float TranslationSpeed = 10;
        public float ZoomSpeed = 10;

        public float ZoomMax = 10;
        public float ZoomMin = 10;

        public Vector3 CurrentPosition { get { return transform.position; } }

        float currentZoom;

        Vector3 startingPosition;
        private void Awake()
        {
            startingPosition = transform.position;
        }

        public void Translate(Vector3 _objectivePosition)
        {
            float x = Mathf.Lerp(transform.position.x, _objectivePosition.x, TranslationSpeed);
            float y = Mathf.Lerp(transform.position.y, _objectivePosition.y, TranslationSpeed);
            float z = Mathf.Lerp(transform.position.y, _objectivePosition.y, TranslationSpeed);

            Vector3 headingPoint = new Vector3(x, y, z);
            transform.position = headingPoint;
        }

        public void Zoom(float _zoomDelta)
        {
            if (currentZoom != _zoomDelta)
            {
                currentZoom += Mathf.Lerp(currentZoom, _zoomDelta, ZoomSpeed);
                if (currentZoom < ZoomMin)
                    currentZoom = ZoomMin;
                if (currentZoom > ZoomMax)
                    currentZoom = ZoomMax;

                Translate(startingPosition + Vector3.forward * currentZoom);
            }

        }
    }
}
