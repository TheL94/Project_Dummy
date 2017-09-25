using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

namespace DumbProject.Generic
{
    public class AudioManager : MonoBehaviour
    {
        [EventRef]
        public string inputSound = "event:/TestEvent";
        EventInstance testMusic;

        public void Init()
        {
            testMusic = RuntimeManager.CreateInstance(inputSound);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                testMusic.start();
            if (Input.GetKeyDown(KeyCode.S))
                testMusic.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}

