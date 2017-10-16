using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlexibleUI
{
    public class FlexibleUIManager : MonoBehaviour
    {
        /// <summary>
        /// Forza la UI verticale, per debug solo su pc.
        /// </summary>
        public static bool ForceVerticalUI = true;

        static ScreenOrientation deviceOrientation = ScreenOrientation.Unknown;
        /// <summary>
        /// Orientamento del dispositivo
        /// </summary>
        public static ScreenOrientation DeviceCurrentOrientation { get { return Screen.orientation; } }

        /// <summary>
        /// Risoluzione massima dello schermo
        /// </summary>
        public static Vector2 DeviceReferenceResolution
        {
            get
            {
                if (SystemInfo.deviceType == DeviceType.Desktop)
                {
                    if (ForceVerticalUI)
                        return new Vector2(Screen.currentResolution.height, Screen.currentResolution.width);
                    else
                        return new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
                }
                else
                {
                    if (DeviceCurrentOrientation == ScreenOrientation.Portrait || DeviceCurrentOrientation == ScreenOrientation.PortraitUpsideDown)
                        return new Vector2(Screen.currentResolution.height, Screen.currentResolution.width);
                    else
                        return new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
                }
            }
        }

        /// <summary>
        /// Risoluzione attuale dello schermo
        /// </summary>
        public static Vector2 CurrentResolution
        {
            get
            {
                if (SystemInfo.deviceType == DeviceType.Desktop)
                {
                    if (ForceVerticalUI)
                        return new Vector2(Screen.height, Screen.width);
                    else
                        return new Vector2(Screen.width, Screen.height);
                }
                else
                {
                    if (DeviceCurrentOrientation == ScreenOrientation.Portrait || DeviceCurrentOrientation == ScreenOrientation.PortraitUpsideDown)
                        return new Vector2(Screen.height, Screen.width);
                    else
                        return new Vector2(Screen.width, Screen.height);
                }
            }
        }

        private void LateUpdate()
        {
            UpdateUIOrientation();
        }

        /// <summary>
        /// Funzione che scatena l'evento che fa aggiornare la ui se cambia l'orientamento del dispositivo
        /// </summary>
        #region API
        public static void UpdateUIOrientation()
        {
            if (deviceOrientation != DeviceCurrentOrientation)
            {
                if (SystemInfo.deviceType == DeviceType.Desktop)
                {
                    if (ForceVerticalUI)
                        deviceOrientation = ScreenOrientation.Portrait;
                    else
                        deviceOrientation = ScreenOrientation.Landscape;
                }
                else if (SystemInfo.deviceType == DeviceType.Handheld)
                {
                    deviceOrientation = DeviceCurrentOrientation;
                }
                OnScreenOrientationChange();
            }
        }

        /// <summary>
        /// Funzione che setta i dati in base all'orientamento e al dispositivo
        /// </summary>
        /// <param name="_rcTransform"></param>
        /// <param name="_verticalData"></param>
        /// <param name="_horizontalData"></param>
        public static void SetRectTransformParametersByData(RectTransform _rcTransform, FlexibleUIData _verticalData, FlexibleUIData _horizontalData)
        {
            RectTranformData values = null;
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                if (ForceVerticalUI)
                    values = _verticalData.RectTranformValues;
                else
                    values = _horizontalData.RectTranformValues;
            }
            else if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                if (DeviceCurrentOrientation == ScreenOrientation.Landscape || DeviceCurrentOrientation == ScreenOrientation.LandscapeLeft || DeviceCurrentOrientation == ScreenOrientation.LandscapeRight)
                    values = _horizontalData.RectTranformValues;
                else if (DeviceCurrentOrientation == ScreenOrientation.Portrait || DeviceCurrentOrientation == ScreenOrientation.PortraitUpsideDown)
                    values = _verticalData.RectTranformValues;
            }

            if (values != null)
                SetRectTransformParametersByValues(_rcTransform, values.AnchorMin, values.AnchorMax, values.OffsetMin, values.OffsetMax);
            else
                Debug.LogWarning("Device Type Not Valid");
        }

        /// <summary>
        /// Funzione che setta la rect transform
        /// </summary>
        /// <param name="_rcTransform"></param>
        /// <param name="_anchorMin"></param>
        /// <param name="_anchorMax"></param>
        /// <param name="_offsetMin"></param>
        /// <param name="_offsetMax"></param>
        public static void SetRectTransformParametersByValues(RectTransform _rcTransform, Vector2 _anchorMin, Vector2 _anchorMax, Vector2 _offsetMin, Vector2 _offsetMax)
        {
            _rcTransform.anchorMin = _anchorMin;
            _rcTransform.anchorMax = _anchorMax;
            _rcTransform.offsetMin = _offsetMin;
            _rcTransform.offsetMax = _offsetMax;
        }
        #endregion

        #region LayoutEvent
        /// <summary>
        /// Delegato che gestisce gli eventi relativi al layout
        /// </summary>
        public delegate void LayoutEvent();
        /// <summary>
        /// Evento che viene scatenato al cambio di orientamento del dispositivo
        /// </summary>
        public static LayoutEvent OnScreenOrientationChange;
        #endregion
    }

    [System.Serializable]
    public class RectTranformData
    {
        public Vector2 AnchorMin;
        public Vector2 AnchorMax;
        public Vector2 OffsetMin;
        public Vector2 OffsetMax;

        public RectTranformData(Vector2 _anchorMin, Vector2 _anchorMax, Vector2 _offsetMin, Vector2 _offsetMax)
        {
            AnchorMin = _anchorMin;
            AnchorMax = _anchorMax;
            OffsetMin = _offsetMin;
            OffsetMax = _offsetMax;
        }
    }
}