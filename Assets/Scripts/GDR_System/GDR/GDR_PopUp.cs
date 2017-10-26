using UnityEngine;
using UnityEngine.UI;

namespace DumbProject.GDR_System
{
    public class GDR_PopUp : MonoBehaviour
    {
        public Text PopUpText;

        private void Update()
        {
            transform.LookAt(-Camera.main.transform.position);
        }

        public void ShowText(string _text, Color _color)
        {
            PopUpText.color = _color;
            PopUpText.text = _text;
        }
    }
}
