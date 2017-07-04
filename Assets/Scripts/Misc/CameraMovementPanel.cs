using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DumbProject.Generic;

namespace DumbProject
{
    public class CameraMovementPanel : MonoBehaviour, IDragHandler
    {

        Vector3 firstPosition;
        Vector3 secondPosition;
        float xAngle = 0;
        float yAngle = 0;
        float xTempAngle = 0;
        float yTempAngle = 0;




        public void OnDrag(PointerEventData eventData)
        {
            Vector2 direction = (eventData.position - eventData.pressPosition) * GameManager.I.CameraVelocity;
            GameManager.I.CameraController.MoveCamera(-direction);
        }

        void Rotation()
        {

        }

        private void Update()
        {
            Touch[] touchs = Input.touches;

            if (touchs.Length > 1)
            {
                //Vector2 direction = (touchs[1].position - touchs[1].
                if (touchs[1].phase == TouchPhase.Began)
                {
                    firstPosition = touchs[1].position;
                    xTempAngle = xAngle;
                    yTempAngle = yAngle;
                }

                if (touchs[1].phase == TouchPhase.Moved)
                {
                    //secondPosition = touchs[1].position;
                    ////Mainly, about rotate camera. For example, for Screen.width rotate on 180 degree
                    //xAngle = xTempAngle + (secondPosition.x - firstPosition.x) * 180.0 / Screen.width;
                    //yAngle = yAngTemp - (secondpoint.y - firstpoint.y) * 90.0 / Screen.height;
                    ////Rotate camera
                    //this.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0);
                }
            }
        }

    }
}
