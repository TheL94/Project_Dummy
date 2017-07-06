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
        PointerEventData pointerData;
        Vector2 positiontoHead;

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

            positiontoHead = eventData.pressPosition;
            //HeadPointer(eventData.pressPosition);
            Debug.Log("sto draggando");
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
                        GameManager.I.CameraController.ZoomTheCamera(touchesDistance);
                    }
                    else
                    {
                        // zoom out
                        GameManager.I.CameraController.ZoomTheCamera(-touchesDistance);
                    }
                }
            }
        }

        void HeadPointer()
        {
            if (!isPointerHeading)
                return;

            /*(pointerData.position - pointerData.pressPosition) * GameManager.I.CameraVelocity;*/
            if (positiontoHead.x != 0 || positiontoHead.y != 0)
            {
                float distance = CalculateDistanceInWorld();


                //Vector3 temp = new Vector3((positiontoHead.x - pointerData.position.x), 0, (positiontoHead.y - pointerData.position.y));
                //GameManager.I.CameraController.MoveCamera(temp.normalized); 
            }
        }

        float CalculateDistanceInWorld()
        {
            float ScreenDistance = Vector2.Distance(positiontoHead, pointerData.position);

            Vector2 A1;
            Vector2 A2;

            A1.x = pointerData.position.x * 776 / 294;
            A1.y = pointerData.position.y * 598 / 294;

            A2.x = positiontoHead.x * 776 / 294;
            A2.y = positiontoHead.y * 598 / 294;

            return Vector2.Distance(A1, A2);
        }

        public void OnScroll(PointerEventData eventData)
        {
            if(eventData.scrollDelta.y > 0)
            {
                GameManager.I.CameraController.ZoomTheCamera(eventData.scrollDelta.y);
            }

            if (eventData.scrollDelta.y < 0)
            {
                GameManager.I.CameraController.ZoomTheCamera(eventData.scrollDelta.y);
            }

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isPointerHeading = true;
            pointerData = eventData;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPointerHeading = false;
        }
    }
}
