using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class UIEndGameplayController : UIChanger
    {
        public Button SkipButton;
        public Text RecapText;

        UIManager uiManager;

        #region API
        public void Init(UIManager _uiManager)
        {
            uiManager = _uiManager;
            ImageToChange = GetComponent<Image>();
            // TODO : inizializzare componenti del menu
        }

        public void Setup()
        {
            // TODO : setup visivo degli elementi (accenderli / spegnerli)
        }

        public void SetRecapText(string _text)
        {
            RecapText.text = _text;
        }
        #endregion

        private void OnEnable()
        {
            SkipButton.onClick.AddListener(() => { GameManager.I.ChageFlowState(Flow.FlowState.ExitGameplay); });
        }

        private void OnDisable()
        {
            SkipButton.onClick.RemoveAllListeners();
        }
    }
}