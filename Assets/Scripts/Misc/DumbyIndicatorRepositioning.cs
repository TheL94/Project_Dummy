using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;


namespace DumbProject.UI
{
    public class DumbyIndicatorRepositioning : MonoBehaviour
    {
        DumbyIndicatorController dumbyIndicatorMovement;

        

        public void Init(DumbyIndicatorController _indicator)
        {
            dumbyIndicatorMovement = _indicator;
            GetComponent<Button>().onClick.AddListener(() => { CameraRepositioning(); });
        }

        private void OnDisable()
        {
            GetComponent<Button>().onClick.RemoveAllListeners();
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