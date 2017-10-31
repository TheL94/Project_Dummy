using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace DumbProject.Generic {
    public class Button_PlayStop : MonoBehaviour
    {
        Dumby dumbyAI;
        bool isInitialized = false;

        public void Init()
        {
            dumbyAI = GameManager.I.Dumby as Dumby;

            isInitialized = true;
        }

        public void ToggleAI()
        {
            if (!isInitialized)
                Init();

            dumbyAI.IsActive = !dumbyAI.IsActive;
        }
    }
}
