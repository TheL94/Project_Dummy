﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using System.Linq;
using UnityEngine.UI;
using DumbProject.GDR_System;

namespace DumbProject.UI
{
    public class IndicatorController : UIChanger
    {
        UIManager uiManager;

        private ScreenOrientation _tempDeviceOrientation;
        private ScreenOrientation TempDeviceOrientation
        {
            get
            {
                return _tempDeviceOrientation;
            }
            set
            {
                if (_tempDeviceOrientation != value)
                {
                    _tempDeviceOrientation = value;
                }
            }
        }

        ScreenOrientation DeviceOrientation
        {
            get
            {
                return uiManager.DeviceCurrentOrientation;
            }
        }
        public bool DrawGizmos;

        public Vector2 CurrentUiFrame;

        bool UiIsVertical;

        Vector2 verticalUiFrame = new Vector2(0, 0.175f);
        Vector2 horizontalUiFrame = new Vector2(0.43f , 0.314f);
        float offset = 5;


        #region Action Image Variables

        Image actionImage;

        IndicatorData Data;

        private ImageState _imageCurrentState;

        public ImageState ImageCurrentState
        {
            get { return _imageCurrentState; }
            set
            {
                _imageCurrentState = value;
                ChangeActionImage(_imageCurrentState);
            }
        }

        #endregion

        [HideInInspector]
        public GameObject Indicator;

        Vector3 targetPosition;


        /// <summary>
        /// Init per istanziare tutti i componenti dell'indicatore
        /// </summary>
        /// <param name="_calledByDumby">Vera se l'init viene chiamato da dumby per impostare il suo indicatore; 
        /// falso se viene chiamato dalla stanza obbiettivo</param>
        public void Init(bool _calledByDumby)
        {
            uiManager = GameManager.I.UIMng;
            Vector3 vector = Camera.main.WorldToScreenPoint(transform.position);
            Data = Instantiate(Resources.Load("Data/UIData/IndicatorData/IndicatorData") as IndicatorData);
            
            // ---------------

            if(_calledByDumby)
                dumbyController = GameManager.I.Dumby.GetComponent<GDR_Controller>();

            // ---------------

            if (uiManager != null)
            {
                GameObject tempActionIndicator;
                Indicator = Instantiate(Data.GameObj, vector + Vector3.up * 70, Quaternion.identity, uiManager.transform);
                Image tempImage = Indicator.AddComponent<Image>();
                Indicator.AddComponent<Button>();
                Indicator.AddComponent<IndicatorRepositioner>().Init(this);

                if (_calledByDumby)
                {
                    tempImage.sprite = Data.ForDumbyIndicator;
                    tempActionIndicator = Instantiate(Data.GameObj, Indicator.transform);
                    actionImage = tempActionIndicator.AddComponent<Image>();

                    actionImage.sprite = Data.ChooseSprite;
                }
                else
                {
                    tempImage.sprite = Data.GenericIndicator;
                }
            }
            SetUIOrientation(GameManager.I.UIMng.DeviceCurrentOrientation);
        }

        private void Update()
        {
            if (uiManager != null)
            {
                _tempDeviceOrientation = DeviceOrientation;
            }

            if (GameManager.I.CurrentState == Flow.FlowState.Pause)
            {
                foreach (Image image in Indicator.GetComponentsInChildren<Image>())
                {
                    image.enabled = false;
                }
            }

            else if (GameManager.I.CurrentState == Flow.FlowState.Gameplay)
            {
                foreach (Image image in Indicator.GetComponentsInChildren<Image>())
                {
                    image.enabled = true;
                }

                if (dumbyController)
                    UpdateIndicatorColor();

                UpdateIndicatorPosition(); 
            }
            else if (GameManager.I.CurrentState == Flow.FlowState.RecapGame)
                Destroy(Indicator);

            if (actionImage != null)
            {
                SetActionImageOrientation();
            }
        }

        /// <summary>
        /// Setta la posizione dell'indicare in UI
        /// </summary>
        void UpdateIndicatorPosition()
        {
            targetPosition = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 70;
            Vector3 iconPos = targetPosition;
            Vector3 iconNewPos = iconPos;

            if (!CheckIfInsideTheScreen(targetPosition))
            {
                iconNewPos = ConstrainPosition(iconPos);
            }

            Indicator.transform.rotation = ConstrainRotation(iconNewPos);
            //Icon.transform.Rotate(0, 0, 90);
            Indicator.transform.position = iconNewPos;
        }

        #region TestRegion
        

        GDR_Controller dumbyController;
        
        void UpdateIndicatorColor()
        {
            if((dumbyController.GetLife() / dumbyController.GetMaxLife())> 0.75f)
                Indicator.GetComponent<Image>().color = Color.green;
            else if ((dumbyController.GetLife() / dumbyController.GetMaxLife()) > 0.35f)
                Indicator.GetComponent<Image>().color = Color.yellow;
            else
                Indicator.GetComponent<Image>().color = Color.red;

        }

        #endregion

        /// <summary>
        /// Controlla se l'indicatore di Dumby si trova all'interno dello schermo
        /// </summary>
        /// <returns>Ritorna quale asse si trova fuori dallo schermo</returns>
        bool CheckIfInsideTheScreen(Vector2 pos)
        {
            if (pos.x > 0 && pos.y > 0 && pos.x < Screen.width && pos.y < Screen.height)
                if (!(pos.x > CurrentUiFrame.x && pos.y < CurrentUiFrame.y))
                    return true;

            return false;
        }

        Vector3 ConstrainPosition(Vector3 iconPos)
        {
            Vector3 iconNewPos = iconPos;

            if (iconPos.x >= Screen.width - offset)
                if (iconPos.y < CurrentUiFrame.y + offset + float.Epsilon)
                    iconNewPos = new Vector3(Screen.width - offset, CurrentUiFrame.y, 0);
                else if (iconPos.y >= Screen.height - offset)
                    iconNewPos = new Vector3(Screen.width - offset, Screen.height - offset, 0);
                else
                    iconNewPos = new Vector3(Screen.width - offset, targetPosition.y, 0);

            if (iconPos.x > CurrentUiFrame.x - offset && iconPos.x < Screen.width - offset)
                if (iconPos.y <= CurrentUiFrame.y + offset + float.Epsilon)
                    iconNewPos = new Vector3(targetPosition.x, CurrentUiFrame.y - offset, 0);
                else if (iconPos.y >= Screen.height - offset)
                    iconNewPos = new Vector3(targetPosition.x, Screen.height - offset, 0);
                else
                    iconNewPos = new Vector3(targetPosition.x, targetPosition.y, 0);

            if (iconPos.x <= CurrentUiFrame.x - offset && iconPos.x >= offset)
                if (iconPos.y <= offset + float.Epsilon)
                    iconNewPos = new Vector3(targetPosition.x, offset, 0);
                else if (iconPos.y >= Screen.height - offset)
                    iconNewPos = new Vector3(targetPosition.x, Screen.height - offset, 0);
                else
                    iconNewPos = new Vector3(targetPosition.x, targetPosition.y, 0);

            if (iconPos.x < offset)
            {
                if (!UiIsVertical)
                {
                    if (iconPos.y <= offset + float.Epsilon)
                        iconNewPos = new Vector3(offset, offset, 0);
                    else if (iconPos.y >= Screen.height - offset)
                        iconNewPos = new Vector3(offset, Screen.height - offset, 0);
                    else
                        iconNewPos = new Vector3(offset, targetPosition.y, 0);
                }
                else
                {
                    if (iconPos.y <= CurrentUiFrame.y + float.Epsilon)
                        iconNewPos = new Vector3(offset, CurrentUiFrame.y, 0);
                    else if (iconPos.y >= Screen.height - offset)
                        iconNewPos = new Vector3(offset, Screen.height - offset, 0);
                    else
                        iconNewPos = new Vector3(offset, targetPosition.y, 0);
                }
            }

            return iconNewPos;
        }

        /// <summary>
        /// Set the Icon orientation in order to look the target
        /// </summary>
        Quaternion ConstrainRotation(Vector3 iconPos)
        {
            Vector3 direction = iconPos - new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Quaternion newRotation;
            newRotation = Quaternion.LookRotation(Vector3.forward, direction);

            return newRotation;
        }

        void SetActionImageOrientation()
        {
            actionImage.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }


        private void OnDrawGizmos()
        {
            if (DrawGizmos)
            {
                Gizmos.color = Color.magenta;

                Gizmos.DrawLine(CurrentUiFrame, new Vector3(Screen.currentResolution.width, CurrentUiFrame.y, 0));
                Gizmos.DrawLine(CurrentUiFrame, new Vector3(CurrentUiFrame.x, 0, 0));
            }
        }

        public override void SetUIOrientation(ScreenOrientation _orientation)
        {
            if (GameManager.I.DeviceEnvironment == DeviceType.Desktop)
            {
                if (GameManager.I.UIMng.ForceVerticalUI)
                {
                    CurrentUiFrame = new Vector2(Screen.width * verticalUiFrame.x, Screen.height * verticalUiFrame.y);
                    UiIsVertical = true;
                }
                else
                {
                    CurrentUiFrame = new Vector2(Screen.width * horizontalUiFrame.x, Screen.height * horizontalUiFrame.y);
                    UiIsVertical = false;
                }
            }
            else
            {
                if (_orientation == ScreenOrientation.Portrait || _orientation == ScreenOrientation.PortraitUpsideDown)
                {
                    CurrentUiFrame = new Vector2(Screen.width * verticalUiFrame.x, Screen.height * verticalUiFrame.y);
                    UiIsVertical = true;
                }

                else
                {
                    CurrentUiFrame = new Vector2(Screen.width * horizontalUiFrame.x, Screen.height * horizontalUiFrame.y);
                    UiIsVertical = false;
                }
            }
        }

        /// <summary>
        /// Change the sprite of the Indicator
        /// </summary>
        /// <param name="_State">State of dumby to change the icon</param>
        void ChangeActionImage(ImageState _State)
        {
            switch (_State)
            {
                case ImageState.Walk:
                    actionImage.sprite = Data.WalkSprite;
                    break;
                case ImageState.Fight:
                    actionImage.sprite = Data.FightSprite;
                    break;
                case ImageState.Choose:
                    actionImage.sprite = Data.ChooseSprite;
                    break;
                case ImageState.Interact:
                    actionImage.sprite = Data.InteractSprite;
                    break;
                default:
                    actionImage.sprite = Data.ChooseSprite;
                    break;
            }
        }

        public enum ImageState
        {
            Walk,
            Fight,
            Choose,
            Interact,
        }

    }
}