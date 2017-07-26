using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Generic
{
    public class ForceFacingCamera : MonoBehaviour
    {
        private void Update()
        {
            transform.LookAt(Camera.main.transform.position);
        }
    }
}
