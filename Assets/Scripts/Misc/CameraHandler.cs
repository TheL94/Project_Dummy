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

        // 2 valori, min e max
        public float[] PerspectiveHorizontalZoomBounds;
        public float[] PerspectiveVerticalZoomBounds;
        public float[] OrthographicHorizontalZoomBounds;
        public float[] OrthographicVerticalZoomBounds;

        Camera cam;

        Vector3 lastPanPosition;
        Vector3 startPosition;

        public void Setup()
        {
            cam = GetComponentInChildren<Camera>();
            startPosition = transform.position;
        }

        public void Init()
        {

            if (startPosition != transform.position)
                transform.position = startPosition;

            if (cam.orthographic)
            {
                if (SystemInfo.deviceType == DeviceType.Desktop)
                {
                    if (GameManager.I.UIMng.ForceVerticalUI)
                        cam.orthographicSize = OrthographicVerticalZoomBounds[1];
                    else
                        cam.orthographicSize = OrthographicHorizontalZoomBounds[1];
                }
                else if (SystemInfo.deviceType == DeviceType.Handheld)
                {
                    if (GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.Landscape || GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.LandscapeLeft || GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.LandscapeRight)
                        cam.orthographicSize = OrthographicHorizontalZoomBounds[1];
                    else if (GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.Portrait || GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.PortraitUpsideDown)
                        cam.orthographicSize = OrthographicVerticalZoomBounds[1];
                }
            }
            else
            {
                if (SystemInfo.deviceType == DeviceType.Desktop)
                {
                    if (GameManager.I.UIMng.ForceVerticalUI)
                        cam.fieldOfView = PerspectiveVerticalZoomBounds[1];
                    else
                        cam.fieldOfView = PerspectiveHorizontalZoomBounds[1];
                }
                else if (SystemInfo.deviceType == DeviceType.Handheld)
                {
                    if (GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.Landscape || GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.LandscapeLeft || GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.LandscapeRight)
                        cam.fieldOfView = PerspectiveHorizontalZoomBounds[1];
                    else if (GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.Portrait || GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.PortraitUpsideDown)
                        cam.fieldOfView = PerspectiveVerticalZoomBounds[1];
                }
            }
        }

        public void ResetValues()
        {
            transform.position = startPosition;
        }


        public void SetLastPanPosition(Vector3 _lastPanPosition)
        {
            lastPanPosition = _lastPanPosition;
        }

        #region Actions
        public void PanCamera(Vector3 _newPanPosition)
        {
            // Determine how much to move the camera
            Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - _newPanPosition);

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
            lastPanPosition = _newPanPosition;
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

            if (cam.orthographic)
            {
                if (SystemInfo.deviceType == DeviceType.Desktop)
                {
                    if (GameManager.I.UIMng.ForceVerticalUI)
                        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - (_offset * zoomSpeed), OrthographicVerticalZoomBounds[0], OrthographicVerticalZoomBounds[1]);
                    else
                        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - (_offset * zoomSpeed), OrthographicHorizontalZoomBounds[0], OrthographicHorizontalZoomBounds[1]);
                }
                else if (SystemInfo.deviceType == DeviceType.Handheld)
                {
                    if (GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.Landscape || GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.LandscapeLeft || GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.LandscapeRight)
                        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - (_offset * zoomSpeed), OrthographicHorizontalZoomBounds[0], OrthographicHorizontalZoomBounds[1]);
                    else if (GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.Portrait || GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.PortraitUpsideDown)
                        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - (_offset * zoomSpeed), OrthographicVerticalZoomBounds[0], OrthographicVerticalZoomBounds[1]);
                }
            }
            else
            {
                if (SystemInfo.deviceType == DeviceType.Desktop)
                {
                    if (GameManager.I.UIMng.ForceVerticalUI)
                        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (_offset * zoomSpeed), PerspectiveVerticalZoomBounds[0], PerspectiveVerticalZoomBounds[1]);
                    else
                        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (_offset * zoomSpeed), PerspectiveHorizontalZoomBounds[0], PerspectiveHorizontalZoomBounds[1]);
                }
                else if (SystemInfo.deviceType == DeviceType.Handheld)
                {
                    if (GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.Landscape || GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.LandscapeLeft || GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.LandscapeRight)
                        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (_offset * zoomSpeed), PerspectiveHorizontalZoomBounds[0], PerspectiveHorizontalZoomBounds[1]);
                    else if (GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.Portrait || GameManager.I.UIMng.DeviceCurrentOrientation == ScreenOrientation.PortraitUpsideDown)
                        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (_offset * zoomSpeed), PerspectiveVerticalZoomBounds[0], PerspectiveVerticalZoomBounds[1]);
                }
            }
        }
        #endregion
    }
}
