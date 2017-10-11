using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DumbProject.UI
{
    public class RoomPanelContainer : MonoBehaviour
    {
        public Sprite VerticalUI;
        public Sprite HorizontalUI;

        Image gamePlayImage;


        List<UIRoomController> _uiSpawns = new List<UIRoomController>();

        public List<UIRoomController> UISpawns { get { return _uiSpawns; } }

        public void Setup(bool _isVertical)
        {
            gamePlayImage = GetComponent<Image>();
            SwitchGamePlayMenuImage(_isVertical);
            foreach (UIRoomController uiCtrl in GetComponentsInChildren<UIRoomController>())
            {
                if(!UISpawns.Contains(uiCtrl))
                    _uiSpawns.Add(uiCtrl);
            }
        }

        /// <summary>
        /// Cambia l'immagine della UI di gioco in base all'orientamento
        /// </summary>
        /// <param name="_isVertical"></param>
        void SwitchGamePlayMenuImage(bool _isVertical)
        {
            if (_isVertical)
                gamePlayImage.sprite = VerticalUI;
            else
                gamePlayImage.sprite = HorizontalUI;
        }

    }
}