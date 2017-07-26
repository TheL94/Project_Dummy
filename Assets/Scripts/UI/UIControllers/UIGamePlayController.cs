using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using UnityEngine.EventSystems;
using System;

namespace DumbProject.UI
{
    public class UIGameplayController : MonoBehaviour
    {

        UIManager uiManager;

        bool isVertical { get { return uiManager.IsVertical; } }

        public GameObject ChildPanel;

        [HideInInspector]
        public GamePlayUIElements GamePlayElements;

        public InventoryContainer InventoryContainer;

        RectTranformValues verticalRectValues;
        RectTranformValues orizontalRectValues;

        #region API

        public void GetRoomPanelControllers()
        {

        }

        public void Init(UIManager _uiManager, RectTranformValues _verticalRectValues, RectTranformValues _orizontalRectValues)
        {
            uiManager = _uiManager;
            verticalRectValues = _verticalRectValues;
            orizontalRectValues = _orizontalRectValues;

            InventoryContainer.Init();
            ChildPanel.SetActive(false);

        }

        public void Setup()
        {
            ChildPanel.SetActive(true);
            //SetVerticalGameUI(isVertical);
        }

        //public void SetVerticalGameUI(bool _isVertical)
        //{
        //    if (InventoryContainer.gameObject.activeInHierarchy || VerticalInventoryContainer.gameObject.activeInHierarchy)
        //    {
        //        bool isPauseActive = false;
        //        if (_isVertical)
        //        {
        //            VerticalInventoryContainer.gameObject.SetActive(true);
        //            if (GamePlayElements.PausePanel != null)
        //            {
        //                if (GamePlayElements.PausePanel.activeInHierarchy)
        //                {
        //                    isPauseActive = true;
        //                    GamePlayElements.PausePanel.SetActive(false);
        //                } 
        //            }
        //            GamePlayElements = VerticalInventoryContainer.GetGamePlayElements();
        //            InventoryContainer.gameObject.SetActive(false);
        //            if (isPauseActive)                                          // Se il pannello di pausa è
        //                GamePlayElements.PausePanel.SetActive(true);
        //        }
        //        else
        //        {
        //            InventoryContainer.gameObject.SetActive(true);
        //            if (GamePlayElements.PausePanel != null)
        //            {
        //                if (GamePlayElements.PausePanel.activeInHierarchy)
        //                {
        //                    isPauseActive = true;
        //                    GamePlayElements.PausePanel.SetActive(false);
        //                } 
        //            }
        //            GamePlayElements = InventoryContainer.GetGamePlayElements();
        //            VerticalInventoryContainer.gameObject.SetActive(false);
        //            if (isPauseActive)
        //                GamePlayElements.PausePanel.SetActive(true);
        //        }
        //    }

        //}

        #region PauseRegion
        /// <summary>
        /// Attiva il pannello della pausa
        /// </summary>
        public void ActivatePause()
        {
            GamePlayElements.PausePanel.SetActive(true);
            GameManager.I.FlowMng.CurrentState = DumbProject.Flow.FlowState.Pause;
        }

        /// <summary>
        /// disattiva il pannello della pausa
        /// </summary>
        public void DeactivatePause()
        {
            GamePlayElements.PausePanel.SetActive(false);
        }

        /// <summary>
        /// Disattiva il pannello della pausa e passa lo stato del flow manager a Gameplay
        /// </summary>
        public void Resume()
        {
            DeactivatePause();
            GameManager.I.DeactivatePauseMode();
        }

        /// <summary>
        /// Esce dallo stato di gameplay per andare nello stato di menu
        /// </summary>
        public void QuitGamePlay()
        {
            DeactivatePause();
            GameManager.I.FlowMng.CurrentState = DumbProject.Flow.FlowState.ExitGameplay;
        }

        #endregion
        #endregion
    }
}