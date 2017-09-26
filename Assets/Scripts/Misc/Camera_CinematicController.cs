using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Camera_CinematicController : MonoBehaviour {

    public float Duration;

	public void Execute(Vector3 _target)
    {
        transform.DOMove(new Vector3(_target.x, transform.position.y, _target.z), Duration).SmoothRewind();
    }
}
