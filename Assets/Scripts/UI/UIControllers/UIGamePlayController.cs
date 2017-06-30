using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.UI;
using DumbProject.Generic;
using UnityEngine.EventSystems;
using System;

public class UIGamePlayController : MonoBehaviour, IDragHandler {

    UIManager uiManager;

    bool isVertical { get { return uiManager.IsVertical; } }

    public GameObject ChildPanel;

    [HideInInspector]
    public GamePlayUIElements GamePlayElements;

    public InventoryContainer InventoryContainer;
    public InventoryContainer VerticalInventoryContainer;

    //[HideInInspector]
    //public RoomPanelController roomPreviewController;
    //[HideInInspector]
    //public InventoryController inventoryController;

    #region API

    public void Init(UIManager _uiManager)
    {
        uiManager = _uiManager;

        InventoryContainer.Init();
        VerticalInventoryContainer.Init();
        ChildPanel.SetActive(false);

    }

    public void Setup()
    {
        ChildPanel.SetActive(true);
        SetVerticalGameUI(isVertical);
    }

    public void SetVerticalGameUI(bool _isVertical)
    {
        if (InventoryContainer.gameObject.activeInHierarchy || VerticalInventoryContainer.gameObject.activeInHierarchy)
        {
            if (_isVertical)
            {
                InventoryContainer.gameObject.SetActive(false);
                VerticalInventoryContainer.gameObject.SetActive(true);
                GamePlayElements = VerticalInventoryContainer.GetGamePlayElements();
            }
            else
            {
                InventoryContainer.gameObject.SetActive(true);
                VerticalInventoryContainer.gameObject.SetActive(false);
                GamePlayElements = InventoryContainer.GetGamePlayElements();
            } 
        }

    }

    #region PauseRegion
    /// <summary>
    /// Attiva il pannello della pausa
    /// </summary>
    public void ActivatePause() {
        GamePlayElements.PausePanel.SetActive(true);
        GameManager.I.FlowMng.CurrentState = DumbProject.Flow.FlowState.Pause;
    }

    /// <summary>
    /// disattiva il pannello della pausa
    /// </summary>
    public void DeactivatePause() {
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

    #region Camera Movement

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    #endregion

    #endregion
}
