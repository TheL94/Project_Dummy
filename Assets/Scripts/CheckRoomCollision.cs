using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CheckRoomCollision : MonoBehaviour {

    Rigidbody2D rigid;
    private bool canAttach;

    public bool CanAttach
    {
        get { return canAttach; }
        private set { canAttach = value; }
    }


    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<CheckRoomCollision>() != null)
            CanAttach = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<CheckRoomCollision>() != null)
            CanAttach = false;
    }
}
