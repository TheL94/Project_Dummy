using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbyIndicatorMovement : MonoBehaviour {

    bool isIncrementig = true;
    Vector3 lowerInitialPosition;
    public float Hight;
    public float Speed;

    private void Start()
    {
        lowerInitialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update () {

        if(!isIncrementig)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 1 * Time.deltaTime * Speed, transform.localPosition.z);
        else
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 1 * Time.deltaTime * Speed, transform.localPosition.z);

        if (transform.localPosition.y >= (lowerInitialPosition.y + Hight))
            isIncrementig = false;
        if (transform.localPosition.y <= (lowerInitialPosition.y))
            isIncrementig = true;

    }
}
