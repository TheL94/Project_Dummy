using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour {

    [EventRef]
    public string inputSound = "event:/TestMusic";
    EventInstance testMusic;
    ParameterInstance tremolo;
    [Range(0f, 1f)]
    public float tremParam;
    // Use this for initialization
    void Start() {
        testMusic = RuntimeManager.CreateInstance(inputSound);
        testMusic.getParameter("TestParam", out tremolo);
    }

	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
            testMusic.start();

        tremolo.setValue(tremParam);
    }
}
