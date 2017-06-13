using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject
{
    public class MenuPanelController : MonoBehaviour
    {
        public GameObject MainMenuPanel;
        public GameObject CreditsPanel;

        #region API

        #region MainMenuAPI

        public void ActivateGamePlay()
        {
            MainMenuPanel.SetActive(false);

        }

        public void ActivateCreditPanel()
        {
            CreditsPanel.SetActive(true);
            MainMenuPanel.SetActive(false);
        }

        public void QuitApplication()
        {
            //Eventuale attivazione di pannelo per conferma uscita dal gioco
            QuitApplication();
        }

        #endregion


        #region CreditsAPI



        #endregion

        #endregion
    }
}