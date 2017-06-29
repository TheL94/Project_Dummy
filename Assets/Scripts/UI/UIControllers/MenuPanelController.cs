using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.UI;
using UnityEngine.UI;

namespace DumbProject
{
    public class MenuPanelController : MonoBehaviour
    {
        bool isVertical;

        public GameObject MainMenuPanel;
        public GameObject VerticalMainMenuPanel;

        public GameObject CreditsPanel;
        public GameObject VerticalCreditsPanel;

        UIManager uiManager;

        #region API

        public void Init(UIManager _uiManager)
        {
            uiManager = _uiManager;
        }

        public void SetVerticalUI(bool _IsVertical)
        {
            isVertical = _IsVertical;
            if (MainMenuPanel.activeInHierarchy || VerticalMainMenuPanel.activeInHierarchy)
            {
                if (isVertical)
                {
                    MainMenuPanel.SetActive(false);
                    VerticalMainMenuPanel.SetActive(true);
                }
                else
                {
                    MainMenuPanel.SetActive(true);
                    VerticalMainMenuPanel.SetActive(false);
                }
            }
            if (CreditsPanel.activeInHierarchy || VerticalCreditsPanel.activeInHierarchy)
            {
                if (isVertical)
                {
                    CreditsPanel.SetActive(false);
                    VerticalCreditsPanel.SetActive(true); 
                }
                else
                {
                    CreditsPanel.SetActive(true);
                    VerticalCreditsPanel.SetActive(false);
                }
            }
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
            if (!isVertical)
            {
                CreditsPanel.SetActive(true);
                MainMenuPanel.SetActive(false); 
            }
            else
            {
                VerticalCreditsPanel.SetActive(true);
                VerticalMainMenuPanel.SetActive(false);
            }
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
            if (!isVertical)
            {
                CreditsPanel.SetActive(false);
                MainMenuPanel.SetActive(true); 
            }
            else
            {
                VerticalCreditsPanel.SetActive(false);
                VerticalMainMenuPanel.SetActive(true);
            }
        }

        #endregion

        #endregion
    }
}