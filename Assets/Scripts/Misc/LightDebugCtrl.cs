using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDebugCtrl : MonoBehaviour {

    public Light light;
	
	void Update ()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            light.intensity += 0.01f;
		if(Input.GetKey(KeyCode.DownArrow))
            light.intensity -= 0.01f;

        if (Input.GetKey(KeyCode.LeftArrow))
            light.range -= 0.01f;
		if(Input.GetKey(KeyCode.RightArrow))
            light.range += 0.01f;
    }
}
