using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using DumbProject.Flow;

namespace DumbProject.Generic {
    public class Camera_CinematicController : MonoBehaviour
    {

        public float Duration;

        public void Execute(Vector3 _start, Vector3 _target)
        {
            Vector3 initialPos = transform.position;
            Vector3 defferedTarget = _target - (_start - transform.position);

            Tween movement = transform.DOMove(defferedTarget, Duration/2);
            movement.OnComplete(() =>
            {
                transform.DOMove(initialPos, Duration/2);
                GameManager.I.ChageFlowState(FlowState.Gameplay);
                GameManager.I.InputHndl.enabled = true;
                movement.Kill();
            });
        }
    }
}