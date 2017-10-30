using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DumbProject.Generic;

namespace DumbProject.UI
{
    public class CameraBackground : UIChanger
    {
        private void Start()
        {
            ImageToChange = GetComponent<Image>();
        }
    }
}