using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DumbProject.Generic;
using Multitouch.EventSystems.EventData;
using Multitouch.EventSystems.Gestures;

namespace DumbProject.CameraController
{
    public class CameraHandler : MonoBehaviour
    {
        public float PanSpeed = 20f;
        public float ZoomSpeedTouch = 0.1f;
        public float ZoomSpeedMouse = 0.5f;
        [Range(1f, 10f)]
        public float BorderLimitDelta = 1f;

        public float[] BoundsX { get { return new float[] { 0f - GameManager.I.MainGridCtrl.CellSize * BorderLimitDelta,
            GameManager.I.MainGridCtrl.GridWidth * GameManager.I.MainGridCtrl.CellSize - GameManager.I.MainGridCtrl.CellSize * BorderLimitDelta }; } }

        public float[] BoundsZ { get { return new float[] { 0f - GameManager.I.MainGridCtrl.CellSize * BorderLimitDelta,
            GameManager.I.MainGridCtrl.GridHeight * GameManager.I.MainGridCtrl.CellSize - GameManager.I.MainGridCtrl.CellSize * BorderLimitDelta }; } }

        public float[] ZoomBounds = new float[] { 10f, 85f };

        private Camera cam;

        private Vector3 lastPanPosition;
        private int panFingerId; // Touch mode only

        private bool wasZoomingLastFrame; // Touch mode only
        private Vector2[] lastZoomPositions; // Touch mode only

        void Start()
        {
            cam = GetComponent<Camera>();
        }

        #region Touch Input
        public void TouchPanning(SimpleGestures _sender, MultiTouchPointerEventData _eventData, Vector2 _delta)
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
                PanCamera(touch.position);
            }
        }

        public void PinchToZoom(SimpleGestures _sender, MultiTouchPointerEventData _eventData, Vector2 _delta)
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

                ZoomCamera(offset, ZoomSpeedTouch);

                lastZoomPositions = newPositions;
            }
        }
        #endregion

        #region Mouse Input
        public void OnClickEvent(PointerEventData _eventData)
        {
            // On mouse down, capture it's position.
            lastPanPosition = _eventData.pressPosition;
        }

        public void OnDragEvent(PointerEventData _eventData)
        {
            // If the mouse is still down, pan the camera.
            PanCamera(_eventData.position);
        }

        public void OnScrollEvent(PointerEventData _eventData)
        {
            // Check for scrolling to zoom the camera
            float scroll = _eventData.scrollDelta.y;
            ZoomCamera(scroll, ZoomSpeedMouse);
        }
        #endregion

        #region Actions
        void PanCamera(Vector3 newPanPosition)
        {
            // Determine how much to move the camera
            Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
            Vector3 move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);

            // Perform the movement
            transform.Translate(move, Space.World);

            // Ensure the camera remains within bounds.
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
            pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
            transform.position = pos;

            // Cache the position
            lastPanPosition = newPanPosition;
        }

        void ZoomCamera(float offset, float speed)
        {
            if (offset == 0)
                return;

            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
        }
        #endregion
    }
}
