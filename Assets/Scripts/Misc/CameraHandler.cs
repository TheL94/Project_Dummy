using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DumbProject.Generic
{
    public class CameraHandler : MonoBehaviour
    {
        public float TouchPanSpeed = 20f;
        public float ZoomSpeedTouch = 0.1f;

        public float MousePanSpeed = 20f;
        public float ZoomSpeedMouse = 0.5f;
        [Range(1f, 10f)]
        public float BorderLimitDelta = 1f;

        public float[] BoundsX { get { return new float[] { 0f - GameManager.I.MainGridCtrl.CellSize * BorderLimitDelta,
            GameManager.I.MainGridCtrl.GridWidth * GameManager.I.MainGridCtrl.CellSize - GameManager.I.MainGridCtrl.CellSize * BorderLimitDelta }; } }

        public float[] BoundsZ { get { return new float[] { 0f - GameManager.I.MainGridCtrl.CellSize * BorderLimitDelta,
            GameManager.I.MainGridCtrl.GridHeight * GameManager.I.MainGridCtrl.CellSize - GameManager.I.MainGridCtrl.CellSize * BorderLimitDelta }; } }

        public float[] ZoomBounds = new float[] { 10f, 85f };

        Camera cam;

        public void Init()
        {
            cam = GetComponentInChildren<Camera>();
        }

        #region Actions
        public void PanCamera(Vector3 _lastPanPosition, Vector3 _newPanPosition)
        {
            // Determine how much to move the camera
            Vector3 offset = cam.ScreenToViewportPoint(_lastPanPosition - _newPanPosition);

            float panSpeed;
            if (GameManager.I.DeviceEnvironment == DeviceType.Handheld)
                panSpeed = TouchPanSpeed;
            else
                panSpeed = MousePanSpeed;
            Vector3 move = new Vector3(offset.x * panSpeed, 0, offset.y * panSpeed);
            //compensate the eventual camera rotation;
            move = Quaternion.Euler(0f, cam.transform.rotation.eulerAngles.y, 0f) * move;
            // Perform the movement
            transform.Translate(move, Space.World);

            // Ensure the camera remains within bounds.
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
            pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
            transform.position = pos;

            // Cache the position
            _lastPanPosition = _newPanPosition;
        }

        public void ZoomCamera(float _offset)
        {
            if (_offset == 0)
                return;

            float zoomSpeed;

            if (GameManager.I.DeviceEnvironment == DeviceType.Handheld)
                zoomSpeed = ZoomSpeedTouch;
            else
                zoomSpeed = ZoomSpeedMouse;

            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (_offset * zoomSpeed), ZoomBounds[0], ZoomBounds[1]);
        }
        #endregion
    }
}
