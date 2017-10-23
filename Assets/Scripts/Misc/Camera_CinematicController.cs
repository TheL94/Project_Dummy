using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using DumbProject.Flow;

namespace DumbProject.Generic {
    public class Camera_CinematicController : MonoBehaviour
    {

        public float Duration;
        Plane gridLevel;
        Ray centralRay;
        Vector2 screenCenter;

        public void Init()
        {
            gridLevel = new Plane(Vector3.up, GameManager.I.MainGridCtrl.transform.position.y + GameManager.I.MainGridCtrl.GridOffsetY);
            screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        }

        public void Execute(Vector3 _start, Vector3 _target)
        {
            Vector3 initialPos = transform.position;
            Vector3 defferedTarget = _target - (_start - transform.position);

            Tween movement = transform.DOMove(defferedTarget, Duration/2);
            movement.OnComplete(() =>
            {
                transform.DOMove(initialPos, Duration/2).OnComplete(() => {
                GameManager.I.ChageFlowState(FlowState.Gameplay);
                GameManager.I.InputHndl.enabled = true;
                movement.Kill();
                });
            });
        }

        /// <summary>
        /// Move the camera in a specific position
        /// </summary>
        /// <param name="_target">The position to reach</param>
        public void MoveTo(Vector3 _target)
        {
            Vector3 startPosition = new Vector3();

            centralRay = Camera.main.ScreenPointToRay(screenCenter);
            float distance;
            if (gridLevel.Raycast(centralRay, out distance))
                startPosition = centralRay.GetPoint(distance);

            Vector3 defferedTarget = _target - (startPosition - transform.position);

            transform.DOMove(defferedTarget, Duration / 4);
        }
    }
}