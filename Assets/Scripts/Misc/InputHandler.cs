using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Generic
{
    public class InputHandler : MonoBehaviour
    {
        Vector3 lastPanPosition;
        int panFingerId; // Touch mode only

        bool wasZoomingLastFrame; // Touch mode only
        Vector2[] lastZoomPositions; // Touch mode only

        CameraHandler cameraHandler;

        public void Init(CameraHandler _camera)
        {
            cameraHandler = _camera;
        }

        void Update()
        {
            if(GameManager.I.IsTouchAvailable)
            {
                HandleTouch();
            }
            else
            {
                HandleMouse();
            }
        }

        #region Touch Input
        public void HandleTouch()
        {
            if (Input.touchCount == 1)
            {
                TouchPanning();
            }
            else if (Input.touchCount == 2)
            {
                PinchToZoom();
            }
            else
            {
                wasZoomingLastFrame = false;
            }
        }

        public void TouchPanning()
        {
            wasZoomingLastFrame = false;

            // If the touch began, capture its position and its finger ID.
            // Otherwise, if the finger ID of the touch doesn't match, skip it.
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = touch.position;
                panFingerId = touch.fingerId;
            }
            else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
            {
                cameraHandler.PanCamera(lastPanPosition, touch.position);
            }
        }

        public void PinchToZoom()
        {
            Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
            if (!wasZoomingLastFrame)
            {
                lastZoomPositions = newPositions;
                wasZoomingLastFrame = true;
            }
            else
            {
                // Zoom based on the distance between the new positions compared to the 
                // distance between the previous positions.
                float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                float offset = newDistance - oldDistance;

                cameraHandler.ZoomCamera(offset);

                lastZoomPositions = newPositions;
            }
        }
        #endregion

        #region Mouse Input
        void HandleMouse()
        {
            // On mouse down, capture it's position.
            // Otherwise, if the mouse is still down, pan the camera.
            if (Input.GetMouseButtonDown(0))
            {
                lastPanPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                cameraHandler.PanCamera(lastPanPosition, Input.mousePosition);
            }

            // Check for scrolling to zoom the camera
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            cameraHandler.ZoomCamera(scroll);
        }
        #endregion
    }
}




