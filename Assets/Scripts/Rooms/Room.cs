using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Framework.Grid;

namespace Framework.Rooms
{
    public class Room : MonoBehaviour
    {

        public List<Cell> RoomCells;

        [HideInInspector]
        public RoomMovment RoomMovment;

        Vector3 _startPosition;
        public Vector3 StartPosition
        {
            get { return _startPosition; }
            set
            {
                if (_startPosition == new Vector3(0, 0, 0))
                {
                    _startPosition = value;
                }
            }
        }

        bool canRotate = true;

        private void Awake()
        {
            RoomCells = GetComponentsInChildren<Cell>().ToList();
            RoomMovment = GetComponent<RoomMovment>();
        }

        public void Init()
        {
            RoomMovment.Init(this);
            StartPosition = transform.position;
        }

        public void Rotate()
        {
            if (canRotate)
            {
                canRotate = false;
                transform.DORotate(transform.up * 90f, 0.5f, RotateMode.LocalAxisAdd).OnComplete(() => { canRotate = true; });
            }
        }
    }
}