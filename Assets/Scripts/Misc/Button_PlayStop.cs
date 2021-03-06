﻿using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace DumbProject.Generic {
    public class Button_PlayStop : MonoBehaviour
    {
        Dumby dumbyAI;
        bool isInitialized = false;

        public float Timer = 40f;
        float privateTimer;
        public float StopTimer = 10f;

        private float _stopTimer;

        public Image loadingImage;

        bool canToggle
        {
            get
            {
                if (privateTimer <= 0)
                    return true;
                else
                    return false;
            }
        }
        bool isToggled { get { return !dumbyAI.IsActive; } }

        public void Init()
        {
            dumbyAI = GameManager.I.Dumby as Dumby;
            privateTimer = Timer;
            _stopTimer = StopTimer;
            isInitialized = true;
            loadingImage.fillAmount = 1f;
        }

        private void Update()
        {
            if (!isInitialized)
                return;

            if (privateTimer >= 0)
            {
                privateTimer -= Time.deltaTime;
                loadingImage.fillAmount = privateTimer / Timer; 
            }
            if (isToggled)
                _stopTimer -= Time.deltaTime; 

            if(_stopTimer <= 0)
            {
                dumbyAI.IsActive = true;
                privateTimer = Timer - _stopTimer;
                _stopTimer = StopTimer;
            }
        }

        public void ToggleAI()
        {
            if (!canToggle || isToggled)
                return;

            dumbyAI.IsActive = !dumbyAI.IsActive;
            _stopTimer = StopTimer;
            loadingImage.fillAmount = 1f;
        }
    }
}
