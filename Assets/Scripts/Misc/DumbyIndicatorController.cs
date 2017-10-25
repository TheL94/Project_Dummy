using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using UnityEngine.UI;
using FlexibleUI;

namespace DumbProject.UI
{
    public class DumbyIndicatorController : IndicatorController
    {
        private IconStatus _currentStatus;

        Image iconImage;

        public IconStatus CurrentStatus
        {
            get { return _currentStatus; }
            set
            {
                _currentStatus = value;
                SetIconImage();
            }
        }

        public override void OnStart()
        {
            iconImage = GetComponent<Image>();
            Icon.AddComponent<DumbyIndicatorRepositioning>().Init(this);
        }

        void SetIconImage()
        {
            if (iconImage != null)
            {
                switch (CurrentStatus)
                {
                    case IconStatus.Walk:
                        // iconImage = walkImage
                        break;
                    case IconStatus.Fight:
                        // iconImage = fightImage
                        break;
                    case IconStatus.Interact:
                        break;
                    case IconStatus.FindingPath:
                        break;
                    default:
                        break;
                }
            }
        }


        public enum IconStatus
        {
            Walk,
            Fight,
            Interact,
            FindingPath,
        }
    }
}