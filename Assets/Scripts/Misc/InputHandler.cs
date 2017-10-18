using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.UI;

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

        bool isDraggingCamera;

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
                if(Input.touchCount == 1)
                    TouchPanning();
                else if(Input.touchCount == 2)
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

        #region Touch Input
        void TouchPanning()
        {
            wasZoomingLastFrame = false;
            UIRoomController roomPreview;
            // If the touch began, capture its position and its finger ID.
            // Otherwise, if the finger ID of the touch doesn't match, skip it.
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && !isDraggingCamera)
            {
                clickOrigin = touch.position;
                panFingerId = touch.fingerId;
                if (!GameManager.I.UIMng.GamePlayCtrl.RoomPanelContainer.CheckIfInputIsForRoomPreviews(clickOrigin, out roomPreview))
                    cameraHandler.SetLastPanPosition(touch.position);
            }
            else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
            {
                if (GameManager.I.UIMng.GamePlayCtrl.RoomPanelContainer.CheckIfInputIsForRoomPreviews(clickOrigin, out roomPreview))
                {
                    if (roomPreview != null)
                    {
                        roomPreview.OnDrag(touch.position);
                    }
                }
                else
                {
                    cameraHandler.PanCamera(touch.position);
                    isDraggingCamera = true;
                }
            }
            else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Ended)
            {
                if (GameManager.I.UIMng.GamePlayCtrl.RoomPanelContainer.CheckIfInputIsForRoomPreviews(Input.mousePosition, out roomPreview))
                {
                    if (GameManager.I.UIMng.GamePlayCtrl.RoomPanelContainer.CheckIfInputIsForRoomPreviews(clickOrigin, out roomPreview))
                    {
                        if (roomPreview != null)
                        {
                            roomPreview.ActualRoom.RoomMovment.MovingToInitialPosition = true;
                            if (roomPreview.ActualRoom.RoomMovment.RoomInitialPosition == roomPreview.ActualRoom.transform.position)
                                roomPreview.CallRotateRoom();
                            else
                                draggedRoom.CallPlaceRoom();
                        }
                    }
                }
                else
                {
                    if (draggedRoom != null)
                    {
                        draggedRoom.CallPlaceRoom();
                        draggedRoom = null;
                    }
                }
                isDraggingCamera = false;
            }
            else
            {
                isDraggingCamera = false;
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
        float timer = 0.25f;

        UIRoomController draggedRoom;

        void HandleMouse()
        {
            // On mouse down, capture it's position.
            // Otherwise, if the mouse is still down, pan the camera.
            UIRoomController roomPreview;

            if (Input.GetMouseButtonDown(0) && !isDraggingCamera)
            {
                clickOrigin = Input.mousePosition;
                if (!GameManager.I.UIMng.GamePlayCtrl.RoomPanelContainer.CheckIfInputIsForRoomPreviews(clickOrigin, out roomPreview))
                    cameraHandler.SetLastPanPosition(clickOrigin);
            }
            else if (Input.GetMouseButton(0))
            {
                if (GameManager.I.UIMng.GamePlayCtrl.RoomPanelContainer.CheckIfInputIsForRoomPreviews(clickOrigin, out roomPreview))
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        if (roomPreview != null)
                        {
                            draggedRoom = roomPreview;
                            draggedRoom.OnDrag(Input.mousePosition);
                        } 
                    }
                }
                else
                {
                    cameraHandler.PanCamera(Input.mousePosition);
                    isDraggingCamera = true;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (GameManager.I.UIMng.GamePlayCtrl.RoomPanelContainer.CheckIfInputIsForRoomPreviews(Input.mousePosition, out roomPreview))
                {
                    if (GameManager.I.UIMng.GamePlayCtrl.RoomPanelContainer.CheckIfInputIsForRoomPreviews(clickOrigin, out roomPreview))
                    {
                        if(roomPreview != null)
                        {
                            roomPreview.ActualRoom.RoomMovment.MovingToInitialPosition = true;
                            if(roomPreview.ActualRoom.RoomMovment.RoomInitialPosition == roomPreview.ActualRoom.transform.position)
                                roomPreview.CallRotateRoom();
                            else
                                draggedRoom.CallPlaceRoom();
                        }
                    }
                }
                else
                {
                    if (draggedRoom != null)
                    {
                        draggedRoom.CallPlaceRoom();
                        draggedRoom = null;
                    }
                }
                isDraggingCamera = false;
                timer = 0.25f;
            }
            else 
            {
                isDraggingCamera = false;
            }
            
            // Check for scrolling to zoom the camera
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            cameraHandler.ZoomCamera(scroll);
        }
        #endregion
    }
}




