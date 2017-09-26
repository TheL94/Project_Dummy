using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using DumbProject.Flow;

namespace DumbProject.Generic {
    public class Camera_CinematicController : MonoBehaviour {

        public float Duration;

        public void Execute(Vector3 _target)
        {
            Tween move = transform.DOMove(new Vector3(_target.x, transform.position.y, _target.z), Duration);
            //move.SmoothRewind();

            move.OnComplete(() => { GameManager.I.ChageFlowState(FlowState.Gameplay);
                GameManager.I.InputHndl.enabled = true;
            });
        }
    }
}