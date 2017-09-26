using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using DumbProject.Flow;

namespace DumbProject.Generic {
    public class Camera_CinematicController : MonoBehaviour
    {

        public float Duration;

        public void Execute(Vector3 _target)
        {
            Transform initialTransform = transform;

            transform.DOMove(new Vector3(_target.x, initialTransform.position.y, _target.z), Duration / 2).OnComplete(() =>
            {
                transform.DOMove(initialTransform.position, Duration / 2).OnComplete(() =>
                {
                    GameManager.I.ChageFlowState(FlowState.Gameplay);
                    GameManager.I.InputHndl.enabled = true;
                });
            });
        }
    }
}