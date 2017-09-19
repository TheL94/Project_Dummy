using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Generic
{
    public class InputHandler : MonoBehaviour
    {
        CameraHandler cameraHandler;

        Vector3 clickOrigin;

        // ---  Touch mode only ----------
        int panFingerId;
        bool wasZoomingLastFrame;
        Vector2[] lastZoomPositions;
        // -------------------------------

        bool isDragging;

        public void Init(CameraHandler _camera)
        {
            cameraHandler = _camera;
        }

        void Update()
        {
            HandleInput();
        }

        void HandleInput()
        {
            if (GameManager.I.IsGamePaused)
                return;
            if (GameManager.I.IsTouchAvailable)
            {
                if(Input.touchCount == 1 && CheckIfInputIsForCamera(Input.GetTouch(0).position))
                    TouchPanning();
                else if(Input.touchCount == 2 && CheckIfInputIsForCamera(Input.GetTouch(0).position))
                    PinchToZoom();
                else              
                    wasZoomingLastFrame = false;
                // TODO : aggiungere supporto a piu di due tocchi
            }
            else
            {
                HandleMouse();
            }
        }

        bool CheckIfInputIsForCamera(Vector2 _position)
        {
            Vector2 deviceResolution = GameManager.I.UIMng.CurrentResolution;
            Vector2 cameraPanelPercentage = new Vector2(GameManager.I.UIMng.CameraPanel.AnchorMax.x - GameManager.I.UIMng.CameraPanel.AnchorMin.x,
                GameManager.I.UIMng.CameraPanel.AnchorMax.y - GameManager.I.UIMng.CameraPanel.AnchorMin.y);
            Vector2 cameraPanelResolution = new Vector2(deviceResolution.x * cameraPanelPercentage.x, deviceResolution.y * cameraPanelPercentage.y);

            if (_position.x < cameraPanelResolution.x && _position.y < cameraPanelResolution.y)
                return true;
            else
                return false;
        }

        #region Touch Input
        void TouchPanning()
        {
            wasZoomingLastFrame = false;

            // If the touch began, capture its position and its finger ID.
            // Otherwise, if the finger ID of the touch doesn't match, skip it.
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                clickOrigin = touch.position;
                if (CheckIfInputIsForCamera(clickOrigin))
                {
                    panFingerId = touch.fingerId;
                    cameraHandler.SetLastPanPosition(touch.position);
                }
                    
            }
            else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
            {
                if (CheckIfInputIsForCamera(clickOrigin))
                {
                    cameraHandler.PanCamera(touch.position);
                }
            }
        }

        void PinchToZoom()
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

            if (Input.GetMouseButtonDown(0) && !isDragging)
            {
                clickOrigin = Input.mousePosition;
                if (CheckIfInputIsForCamera(clickOrigin))
                {
                    cameraHandler.SetLastPanPosition(Input.mousePosition);
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (CheckIfInputIsForCamera(clickOrigin))
                {
                    cameraHandler.PanCamera(Input.mousePosition);
                    isDragging = true;
                }
            }
            else 
            {
                isDragging = false;
            }
            
            // Check for scrolling to zoom the camera
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            cameraHandler.ZoomCamera(scroll);
        }
        #endregion
    }
}




