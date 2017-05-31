using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Room : MonoBehaviour {

    List<Cell> RoomCells;

    public void Rotate()
    {
        transform.DORotate(Vector3.forward * -90, 0.5f, RotateMode.LocalAxisAdd);
    }
}
