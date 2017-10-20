using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FlexibleUI;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class UIManager : MonoBehaviour
    {
        public bool ForceVerticalUI { get { return FlexibleUIManager.ForceVerticalUI; } }
        public ScreenOrientation DeviceCurrentOrientation { get { return FlexibleUIManager.DeviceCurrentOrientation;  } }
        public Vector2 DeviceReferenceResolution { get { return FlexibleUIManager.DeviceReferenceResolution; } }
        public Vector2 CurrentResolution { get { return FlexibleUIManager.CurrentResolution; } }

        [HideInInspector]
        public UIMenuController MenuController;
        [HideInInspector]
        public UIGamePlayController GamePlayCtrl;
        [HideInInspector]
        public UIEndGameplayController EndGameCtrl;
        [HideInInspector]
        public UICameraPanelController CameraPanel;

        const string PrefabPath = "Prefabs/UI/";

        List<IUIChanger> UIchangers = new List<IUIChanger>();

        private void OnEnable()
        {
            FlexibleUIManager.OnScreenOrientationChange += SetChildrensPanelsOrientation;
        }

        private void OnDisable()
        {
            FlexibleUIManager.OnScreenOrientationChange -= SetChildrensPanelsOrientation;
        }

        public void Init()
        {
            //setta la risoluzione di riferimento del canvas in base a quella dello schermo del dispositivo in uso
            CanvasScaler scaler = GetComponent<CanvasScaler>();
            scaler.referenceResolution = DeviceReferenceResolution;
            // setup camera panel
            CameraPanel = Instantiate(Resources.Load<GameObject>(PrefabPath + "CameraMovementPanel"), transform).GetComponent<UICameraPanelController>();
            // setup menu panel
            MenuController = Instantiate(Resources.Load<GameObject>(PrefabPath + "MenuController"), transform).GetComponent<UIMenuController>();
            MenuController.Init(this);
            // setup gameplay panel
            GamePlayCtrl = Instantiate(Resources.Load<GameObject>(PrefabPath + "GameplayPanel"), transform).GetComponent<UIGamePlayController>();
            GamePlayCtrl.Init(this);
            // endgameplay panel
            EndGameCtrl = Instantiate(Resources.Load<GameObject>(PrefabPath + "EndGamePanel"), transform).GetComponent<UIEndGameplayController>();
            EndGameCtrl.Init(this);

            FlexibleUIManager.UpdateUIOrientation();

            SetChildrensPanelsOrientation();

            MenuController.Setup();
            GamePlayCtrl.Setup();
            EndGameCtrl.Setup();
        }

        public void ActivateMenuPanel(bool _status)
        {
            MenuController.gameObject.SetActive(_status);
        }

        public void ActivateGamePlayPanel(bool _status)
        {
            GamePlayCtrl.gameObject.SetActive(_status);
        }

        public void ActivateEndGameplayPanel(bool _status)
        {
            EndGameCtrl.gameObject.SetActive(_status);
        }

        public void ActivateCameraPanel(bool _status)
        {
            CameraPanel.gameObject.SetActive(_status);
        }

        /// <summary>
        /// Ogni volta che viene scatenato l'evento on screen orientation change, vengono avvisati tutti i pannelli che devono modificare la propria immagine.
        /// </summary>
        void SetChildrensPanelsOrientation()
        {
            foreach (IUIChanger changer in UIchangers)
            {
                changer.SetUIOrientation(DeviceCurrentOrientation);
            }
        }

        /// <summary>
        /// Delegato che gestisce gli eventi relativi al layout
        /// </summary>
        public delegate void LayoutEvent();
        /// <summary>
        /// Evento che viene scatenato al cambio di orientamento del dispositivo
        /// </summary>
        public LayoutEvent OnScreenOrientationChange;
    }

    [System.Serializable]
    public class RectTranformValues
    {
        public Vector2 AnchorMin;
        public Vector2 AnchorMax;
        public Vector2 OffsetMin;
        public Vector2 OffsetMax;

        public RectTranformValues(Vector2 _anchorMin, Vector2 _anchorMax, Vector2 _offsetMin, Vector2 _offsetMax)
        {
            AnchorMin = _anchorMin;
            AnchorMax = _anchorMax;
            OffsetMin = _offsetMin;
            OffsetMax = _offsetMax;
        }
    }
}