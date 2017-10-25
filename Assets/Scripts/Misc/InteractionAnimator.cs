using UnityEngine;
using DG.Tweening;

namespace DumbProject.Generic {
    public class InteractionAnimator : MonoBehaviour
    {
        public float Duration = .5f;
        public float ChestInteractionIntensity = .1f;
        public float OpeningAngle;
        public void OpenAsDoor()
        {
            transform.DOLocalRotateQuaternion(Quaternion.AngleAxis(OpeningAngle, Vector3.up), Duration);
        }

        public void OpenAsChest()
        {
            transform.DOPunchPosition(Vector3.up * ChestInteractionIntensity, Duration);
            transform.DOPunchRotation(Vector3.up * ChestInteractionIntensity, Duration);
        }
    }
}
