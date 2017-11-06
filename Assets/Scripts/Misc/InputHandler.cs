using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.UI;
using UnityEngine.UI;

namespace DumbProject.Generic
{
    public class InputHandler : MonoBehaviour
    {
        CameraHandler cameraHandler;

        Vector3 clickOrigin;

        Text DebugText;

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

        public void SetupLogger()
        {
            DebugText = GameManager.I.UIMng.DebugText;
        }

        void HandleInput()
        {
            if (GameManager.I.IsGamePaused || GameManager.I.CurrentState != Flow.FlowState.Gameplay)
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
                        draggedRoom = roomPreview;
                        draggedRoom.OnDrag(touch.position);
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
                
                if (GameManager.I.UIMng.GamePlayCtrl.RoomPanelContainer.CheckIfInputIsForRoomPreviews(touch.position, out roomPreview))
                {

                    if (GameManager.I.UIMng.GamePlayCtrl.RoomPanelContainer.CheckIfInputIsForRoomPreviews(clickOrigin, out roomPreview))
                    {
                        if (roomPreview != null)
                        {
                            if (roomPreview.ActualRoom.RoomMovment.RoomInitialPosition == roomPreview.ActualRoom.transform.position)
                            {
                                roomPreview.CallRotateRoom();
                            }
                            else
                            {
                                draggedRoom.CallPlaceRoom();
                            }
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

        #region DragOnScreenEdges
        /// <summary>
        /// Return a vector that point perpendicular whith the colliding Edges
        /// </summary>
        /// <param name="_screenPosition"></param>
        /// <param name="_offSet"></param>
        /// <returns></returns>
        Vector2 GetEdgeCollisionVector(Vector2 _screenPosition, float _offSet = .1f)
        {
            Vector2 outcome = Vector2.zero;

            if (_screenPosition.x <= _offSet)
                outcome -= new Vector2(_offSet, 0);

            else if (_screenPosition.x >= Screen.width - _offSet)
                outcome += new Vector2(_offSet, 0);

            else if (_screenPosition.y <= _offSet)
                outcome -= new Vector2(0, _offSet);

            else if (_screenPosition.y >= Screen.height - _offSet)
                outcome += new Vector2(0, _offSet);

            return outcome;
        }
        #endregion
    }
}




