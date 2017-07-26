using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.UI;
using UnityEngine.UI;

namespace DumbProject
{
    public class UIMenuController : MonoBehaviour
    {
        bool isVertical { get {
                if (uiManager)
                    return uiManager.IsVertical;
                else
                    return false;
            }
        }

        public GameObject MainMenuPanel;
        public GameObject VerticalMainMenuPanel;

        public GameObject CreditsPanel;
        public GameObject VerticalCreditsPanel;

        UIManager uiManager;

        public GameObject ChildPanel;


        #region API

        public void Init(UIManager _uiManager)
        {
            uiManager = _uiManager;
        }

        public void Setup()
        {
            ChildPanel.SetActive(true);
            SetVerticalUI(isVertical);
        }

        public void SetVerticalUI(bool _IsVertical)
        {
            if (MainMenuPanel.activeInHierarchy || VerticalMainMenuPanel.activeInHierarchy)
            {
                if (_IsVertical)
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
                if (_IsVertical)
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
            ChildPanel.gameObject.SetActive(false);
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