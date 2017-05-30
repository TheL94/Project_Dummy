using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateRoom : MonoBehaviour {

    float doubleClickStart = 0;
    

    void OnMouseUp()
    {
        if ((Time.time - doubleClickStart) <= 0.3f)
      {
            OnDoubleClick();
            doubleClickStart = -1;
        }
      else
      {
            doubleClickStart = Time.time;
        }
    }

    void OnDoubleClick()
    {
        Debug.Log("Double Clicked!");
        transform.DORotate(Vector3.forward * -90, 0.5f, RotateMode.LocalAxisAdd);
    }

}