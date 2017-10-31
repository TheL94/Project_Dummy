using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace DumbProject.Generic {
    public class Button_PlayStop : MonoBehaviour
    {
        Dumby dumbyAI;
        bool isInitialized = false;

        public float Timer = 40f;
        float privateTimer;
        float StopTimer = 10f;

        private float _stopTimer;

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
        }

        private void Update()
        {
            if (!isInitialized)
                Init();

            privateTimer -= Time.deltaTime;
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
            privateTimer = Timer;
            _stopTimer = StopTimer;
        }
    }
}
