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
        UIManager uiMng;
        protected CameraHandler camHandler;
        protected RectTransform canvasTransform;

        public void Init(UIManager _uiMng, CameraHandler _camHandler)
        {
            uiMng = _uiMng;
            camHandler = _camHandler;
            canvasTransform = transform as RectTransform;
            SetCanvasRect();
        }

        protected void SetCanvasRect()
        {
            if (GameManager.I.DeviceEnvironment == DeviceType.Desktop)
            {
                SetOrizontal();
            }
            else if(GameManager.I.DeviceEnvironment == DeviceType.Handheld)
            {
                if (Screen.orientation == ScreenOrientation.Landscape)
                    SetOrizontal();
                else
                    SetVertical();
            }
        }

        void SetOrizontal()
        {
            canvasTransform.anchorMin = Vector2.zero;
            canvasTransform.anchorMax = new Vector2(0.766f, 1f);
            canvasTransform.offsetMin = new Vector2(0f, 600f);
            canvasTransform.offsetMax = new Vector2(451f, 0f);
        }

        void SetVertical()
        {
            canvasTransform.anchorMin = new Vector2(0f, 0.307f);
            canvasTransform.anchorMax = Vector2.one;
            canvasTransform.offsetMin = new Vector2(6.1f, 600f);
            canvasTransform.offsetMax = new Vector2(639f, -393f);
        }
    }
}