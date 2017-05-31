using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    public void Init(Vector3 _position)
    {
        transform.position = _position;
    }
}
