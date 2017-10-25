using UnityEngine;
using DG.Tweening;

namespace DumbProject.Generic {
    public class DoorOpener : MonoBehaviour
    {
        public float OpeningAngle;
        public void Open()
        {
            transform.DORotateQuaternion(Quaternion.AngleAxis(OpeningAngle, Vector3.up), .5f);
        }
    }
}
