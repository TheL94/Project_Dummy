using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.UI;

namespace DumbProject
{
    public class MenuPanelController : MonoBehaviour
    {
        public GameObject MainMenuPanel;
        public GameObject CreditsPanel;

        UIManager uiManager;

        #region API

        public void Init(UIManager _uiManager)
        {
            uiManager = _uiManager;
        }

        #region MainMenuAPI

        /// <summary>
        /// Disattiva il pannella del menu e imposta lo stato di gameplay nel flowManager
        /// </summary>
        public void ActivateGamePlay()
        {
            gameObject.SetActive(false);
            uiManager.GoInGameplayMode();
        }

        /// <summary>
        /// Disattiva il main menu e attiva i credits
        /// </summary>
        public void ActivateCreditPanel()
        {
            CreditsPanel.SetActive(true);
            MainMenuPanel.SetActive(false);
        }

        public void QuitApplication()
        {
            //Eventuale attivazione di pannelo per conferma uscita dal gioco
            Application.Quit();
        }

        #endregion


        #region CreditsAPI

        /// <summary>
        /// Attiva il pannello del menu disattivando quello dei credit
        /// </summary>
        public void ActivateMenuPanel()
        {
            CreditsPanel.SetActive(false);
            MainMenuPanel.SetActive(true);
        }

        #endregion

        #endregion
    }
}