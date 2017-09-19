using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.UI
{
    public class UICameraPanelController : MonoBehaviour
    {
        public Vector2 AnchorMin { get { return (transform as RectTransform).anchorMin; } }
        public Vector2 AnchorMax { get { return (transform as RectTransform).anchorMax; } }
    }
}
