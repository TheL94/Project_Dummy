using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;


namespace DumbProject.UI
{
    public class DumbyIndicatorRepositioning : MonoBehaviour
    {
        DumbyIndicatorManager dumbyIndicatorMovement;

        public void Init(DumbyIndicatorManager _indicator)
        {
            dumbyIndicatorMovement = _indicator;
        }

        /// <summary>
        /// Take Dumby positin and call the function in Camera Cinematic Controller to move the camera.
        /// </summary>
        public void CameraRepositioning()
        {
            GameManager.I.CameraHndl.GetComponent<Camera_CinematicController>().MoveTo(dumbyIndicatorMovement.transform.position);
        }
    }
}