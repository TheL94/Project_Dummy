using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Room : MonoBehaviour {

    List<Cell> RoomCells;

    [HideInInspector]
    public Vector3 startPosition;

    bool canRotate = true;

    private bool _inPosition = false;

    public bool InPosition
    {
        get { return _inPosition; }
        set {
            _inPosition = value;
            if (_inPosition == true)
                roomManager.InstantiateRoom(startPosition);
        }
    }

    RoomManager roomManager;

    public void Init(RoomManager _roomManager)
    {
        roomManager = _roomManager;
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
