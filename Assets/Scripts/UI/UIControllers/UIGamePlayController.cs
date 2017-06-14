using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.UI;
using DumbProject.Generic;

public class UIGamePlayController : MonoBehaviour {

    UIManager uiManager;

    [HideInInspector]
    public RoomPanelController roomPreviewController;
    public GameObject PausePanel;

    public void Init(UIManager _uiManager)
    {
        uiManager = _uiManager;
        gameObject.SetActive(false);

        roomPreviewController = GetComponentInChildren<RoomPanelController>();
        roomPreviewController.Setup();
    }

    #region PauseRegion
    /// <summary>
    /// Attiva il pannello della pausa
    /// </summary>
    public void ActivatePause() {
        PausePanel.SetActive(true);
    }

    /// <summary>
    /// disattiva il pannello della pausa
    /// </summary>
    public void DeactivatePause() {
        PausePanel.SetActive(false);
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
        GameManager.I.flowMng.CurrentState = DumbProject.Flow.FlowState.ExitGameplay;
    }


    #endregion

}
