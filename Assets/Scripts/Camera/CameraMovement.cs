using System;
using System.Collections;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject
{
    public class CameraMovement : MonoBehaviour
    {
        public Camera MainCamera { get { return Camera.main; } }
        public float TranslationSpeed = 10;
        public float ZoomSpeed = 10;

        public float ZoomMax = 10;
        public float ZoomMin = 10;

        public Vector3 CurrentPosition { get { return transform.position; } }

        float currentZoom;

        public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
        public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.

        Vector3 startingPosition;
        private void Awake()
        {
            startingPosition = transform.position;
        }

        public void Drag()
        {

        }

        public void Zoom()
        {
            if (GameManager.I.DeviceEnviroment == DeviceType.Desktop)
                ScrollZoom(0.1f);
            else if(GameManager.I.DeviceEnviroment == DeviceType.Handheld)
                PinchToZoom();
        }

        void PinchToZoom()
        {
            // If there are two touches on the device...
            if (Input.touchCount == 2)
            {
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // If the camera is orthographic...
                if (MainCamera.orthographic)
                {
                    // ... change the orthographic size based on the change in distance between the touches.
                    MainCamera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                    // Make sure the orthographic size never drops below zero.
                    MainCamera.orthographicSize = Mathf.Max(MainCamera.orthographicSize, 0.1f);
                }
                else
                {
                    // Otherwise change the field of view based on the change in distance between the touches.
                    MainCamera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                    // Clamp the field of view to make sure it's between 0 and 180.
                    MainCamera.fieldOfView = Mathf.Clamp(MainCamera.fieldOfView, 0.1f, 179.9f);
                }
            }
        }

        void ScrollZoom(float _zoomDelta)
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

        void Translate(Vector3 _objectivePosition)
        {
            float x = Mathf.Lerp(transform.position.x, _objectivePosition.x, TranslationSpeed);
            float y = Mathf.Lerp(transform.position.y, _objectivePosition.y, TranslationSpeed);
            float z = Mathf.Lerp(transform.position.y, _objectivePosition.y, TranslationSpeed);

            Vector3 headingPoint = new Vector3(x, y, z);
            transform.position = headingPoint;
        }
    }
}
