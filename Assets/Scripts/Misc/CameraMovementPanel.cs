using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DumbProject.Generic;

namespace DumbProject
{
    public class CameraMovementPanel : MonoBehaviour, IDragHandler, IScrollHandler, IPointerDownHandler, IPointerUpHandler
    {
        bool isPointerHeading = false;
        PointerEventData initialPointerData;
        PointerEventData currentPointerData;
        float touchesDistance;

        #region Rotation Option

        Vector3 firstPosition;
        Vector3 secondPosition;
        float xAngle = 0;
        float yAngle = 0;
        float xTempAngle = 0;
        float yTempAngle = 0;

        void Rotation()
        {
            /// Touchs esiste solo nell'update
            
            //Vector2 direction = (touchs[1].position - touchs[1].
            //if (touchs[1].phase == TouchPhase.Began)
            //{
            //    firstPosition = touchs[1].position;
            //    xTempAngle = xAngle;
            //    yTempAngle = yAngle;
            //}

            //if (touchs[1].phase == TouchPhase.Moved)
            //{
            //    secondPosition = touchs[1].position;
            //    //Mainly, about rotate camera. For example, for Screen.width rotate on 180 degree
            //    xAngle = (float)(xTempAngle + (secondPosition.x - firstPosition.x) * 180.0 / Screen.width);
            //    yAngle = (float)(yTempAngle - (secondPosition.y - firstPosition.y) * 90.0 / Screen.height);
            //    //Rotate camera
            //    Quaternion tempQuaternion = Quaternion.Euler(yAngle, xAngle, 0.0f);
            //    GameManager.I.CameraController.RotateCamera(tempQuaternion);
            //}
        }

        #endregion


        public void OnDrag(PointerEventData eventData)
        {
            currentPointerData = eventData;
            //HeadPointer(eventData.pressPosition);
            //Vector2 direction = (eventData.position - eventData.pressPosition) * GameManager.I.CameraVelocity;
            //GameManager.I.CameraController.MoveCamera(-direction);
        }

        private void Update()
        {
            Zoom();
            HeadPointer();
        }

        void Zoom()
        {
            if (GameManager.I.DeviceEnviroment == DeviceType.Handheld)
            {
                Touch[] touchs = Input.touches;

                if (touchs.Length > 1)
                {
                    float tempDistance = touchesDistance;
                    touchesDistance = Vector3.Distance(touchs[0].position, touchs[1].position);

                    if (touchesDistance > tempDistance)
                    {
                        // zoom in
                        GameManager.I.CameraManager.ZoomTheCamera(touchesDistance);
                    }
                    else
                    {
                        // zoom out
                        GameManager.I.CameraManager.ZoomTheCamera(-touchesDistance);
                    }
                }
            }
        }

        void HeadPointer()
        {
            if (!isPointerHeading || currentPointerData == null)
                return;

            /*(pointerData.position - pointerData.pressPosition) * GameManager.I.CameraVelocity;*/
            if (currentPointerData.position.x != 0 || currentPointerData.position.y != 0)
            {
                float distance = CalculateDistanceInWorld();


                //Vector3 temp = new Vector3((positiontoHead.x - pointerData.position.x), 0, (positiontoHead.y - pointerData.position.y));
                //GameManager.I.CameraController.MoveCamera(temp.normalized); 
            }
        }

        float CalculateDistanceInWorld()
        {
            float ScreenDistance = Vector2.Distance(currentPointerData.position, initialPointerData.position);

            Vector2 A1;
            Vector2 A2;

            A1.x = initialPointerData.position.x * 776 / 294;
            A1.y = initialPointerData.position.y * 598 / 294;

            A2.x = currentPointerData.position.x * 776 / 294;
            A2.y = currentPointerData.position.y * 598 / 294;

            Debug.Log(string.Format("Distanza dello schermo {0}, distanza del trascinamento {1}", ScreenDistance, Vector2.Distance(A1, A2)));
            return Vector2.Distance(A1, A2);
        }

        public void OnScroll(PointerEventData eventData)
        {
            if(eventData.scrollDelta.y > 0)
            {
                GameManager.I.CameraManager.ZoomTheCamera(eventData.scrollDelta.y);
            }

            if (eventData.scrollDelta.y < 0)
            {
                GameManager.I.CameraManager.ZoomTheCamera(eventData.scrollDelta.y);
            }

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isPointerHeading = true;
            initialPointerData = eventData;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPointerHeading = false;
        }

    }
}
