using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Room : MonoBehaviour {

    public List<Cell> RoomCells;

    [HideInInspector]
    public RoomMovment RoomMovment;
    [HideInInspector]
    public Vector3 StartPosition;

    bool canRotate = true;

    private void Awake()
    {
        RoomCells = GetComponentsInChildren<Cell>().ToList();
        RoomMovment = GetComponent<RoomMovment>();
    }

    public void Init()
    {
        RoomMovment.Init(this);
    }

    public void Rotate()
    {
        if (canRotate)
        {
            canRotate = false;
            transform.DORotate(Vector3.forward * -90, 0.5f).OnComplete(() => { canRotate = true; });
        }
    }
}
