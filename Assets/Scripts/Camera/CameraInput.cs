using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DumbProject.CameraController;
using DumbProject.UI;

namespace DumbProject.Generic
{
    public abstract class CameraInput : UIBehaviour
    {
        protected UIManager uiMng;
        protected CameraHandler camHandler;
        protected RectTransform canvasTransform;
        protected UIPositionData positionData;

        public void Init(UIManager _uiMng, CameraHandler _camHandler, UIPositionData _positionData)
        {
            uiMng = _uiMng;
            camHandler = _camHandler;
            positionData = _positionData;
            uiMng.SetRectTransformParameters(transform as RectTransform, _positionData);
        }
    }
}