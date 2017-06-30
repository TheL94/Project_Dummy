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

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 direction = (eventData.position - eventData.pressPosition).normalized;
            GameManager.I.CameraController.MoveCamera(-direction);
        }

        private void Update()
        {
            Touch[] touchs = Input.touches;

            if(touchs.Length > 1)
            {
                //Vector2 direction = (touchs[1].position - touchs[1].
            }

        }
    }
}