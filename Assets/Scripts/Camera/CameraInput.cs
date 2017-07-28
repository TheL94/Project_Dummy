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

        public void Init(UIManager _uiMng, CameraHandler _camHandler)
        {
            uiMng = _uiMng;
            camHandler = _camHandler;
        }
    }
}