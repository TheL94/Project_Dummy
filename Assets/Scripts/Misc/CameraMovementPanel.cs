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
    }
}