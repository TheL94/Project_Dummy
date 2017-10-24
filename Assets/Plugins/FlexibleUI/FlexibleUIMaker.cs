using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace FlexibleUI
{
    #if UNITY_EDITOR
        [ExecuteInEditMode]
    #endif
    [AddComponentMenu("FlexibleUI/Maker")]
    public class FlexibleUIMaker : MonoBehaviour
    {
        public string Path;

        public bool WorkOnlyOnThisObject;
        public bool ChangeExistingData;

        public LayoutOrientation SelectedLayoutOrientation;

        public void CheckFolder()
        {
            #if UNITY_EDITOR
                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                    AssetDatabase.CreateFolder("Assets", "Resources");

                if (!AssetDatabase.IsValidFolder("Assets/Resources/FlexibleUIData"))
                    AssetDatabase.CreateFolder("Assets/Resources", "FlexibleUIData");
            #endif
            Path = "Assets/Resources/FlexibleUIData/";
        }

        public void MakeUI()
        {
            if(Path == null || Path == string.Empty)
                CheckFolder();

            List<RectTransform> rectTransformList = GetRectTransfomList();
            if (rectTransformList == null || rectTransformList.Count == 0)
            {
                Debug.LogWarning("No RectTransform component on this object or on its children !");
            }
            else
            {
                foreach (RectTransform rectT in rectTransformList)
                {
                    FlexibleUIComponent component = rectT.GetComponent<FlexibleUIComponent>();
                    if(component != null)
                    {
                        SetupExistringFlexibleUIComponent(rectT, component);
                    }
                    else
                    {
                        SetupNewFlexibleUIComponent(rectT);
                    }
                }
            }
        }

        void SetupNewFlexibleUIComponent(RectTransform _rectT)
        {
            FlexibleUIComponent component = AddFlexibleUIComponent(_rectT);
            if (Evaluate(_rectT))
            {
                FlexibleUIData asset = CreateFlexibleDataAsset(_rectT.gameObject.name, SelectedLayoutOrientation);
                LinkDataToComponent(component, SetFlexibleDataAsset(_rectT, asset), SelectedLayoutOrientation);
            }
            else
            {
                SetFillParentAnchors(component);
            }
        }

        void SetupExistringFlexibleUIComponent(RectTransform _rectT, FlexibleUIComponent _component)
        {
            if (!Evaluate(_rectT) && _component.FillParentAnchors)
            {
                return;
            }
            else
            {
                if (_component.FillParentAnchors)
                {
                    if (SelectedLayoutOrientation == LayoutOrientation.Vertical)
                    {
                        FlexibleUIData asset = CreateFlexibleDataAsset(_rectT.gameObject.name, SelectedLayoutOrientation);
                        LinkDataToComponent(_component, SetFlexibleDataAsset(_rectT, asset), SelectedLayoutOrientation);

                        asset = CreateFlexibleDataAsset(_rectT.gameObject.name, LayoutOrientation.Horizontal);
                        LinkDataToComponent(_component, SetFlexibleDataAsset(asset, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero), SelectedLayoutOrientation);
                    }
                    else if (SelectedLayoutOrientation == LayoutOrientation.Horizontal)
                    {
                        FlexibleUIData asset = CreateFlexibleDataAsset(_rectT.gameObject.name, SelectedLayoutOrientation);
                        LinkDataToComponent(_component, SetFlexibleDataAsset(_rectT, asset), SelectedLayoutOrientation);

                        asset = CreateFlexibleDataAsset(_rectT.gameObject.name, LayoutOrientation.Vertical);
                        LinkDataToComponent(_component, SetFlexibleDataAsset(asset, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero), SelectedLayoutOrientation);
                    }
                }
                else
                {
                    if (SelectedLayoutOrientation == LayoutOrientation.Vertical)
                    {
                        if (_component.VerticalUIObjectData != null)
                        {
                            if (ChangeExistingData)
                                SetFlexibleDataAsset(_rectT, _component.VerticalUIObjectData);
                        }
                        else
                        {
                            FlexibleUIData asset = CreateFlexibleDataAsset(_rectT.gameObject.name, SelectedLayoutOrientation);
                            LinkDataToComponent(_component, SetFlexibleDataAsset(_rectT, asset), SelectedLayoutOrientation);
                        }

                    }
                    else if (SelectedLayoutOrientation == LayoutOrientation.Horizontal)
                    {
                        if (_component.HorizontalUIObjectData != null)
                        {
                            if (ChangeExistingData)
                                SetFlexibleDataAsset(_rectT, _component.HorizontalUIObjectData);
                        }
                        else
                        {
                            FlexibleUIData asset = CreateFlexibleDataAsset(_rectT.gameObject.name, SelectedLayoutOrientation);
                            LinkDataToComponent(_component, SetFlexibleDataAsset(_rectT, asset), SelectedLayoutOrientation);
                        }
                    }
                }
            }
        }

        List<RectTransform> GetRectTransfomList()
        {
            List<RectTransform> rectTransformList = null;
            if (WorkOnlyOnThisObject)
                rectTransformList = new List<RectTransform>() { GetComponent<RectTransform>() };
            else
                rectTransformList = GetComponentsInChildren<RectTransform>().ToList();

            if(rectTransformList != null)
                rectTransformList.RemoveAll(r => r.GetComponent<Canvas>() == true);

            return rectTransformList;
        }

        bool Evaluate(RectTransform _rectT)
        {
            if(_rectT.anchorMin == Vector2.zero && _rectT.anchorMax == Vector2.one && _rectT.offsetMin == Vector2.zero && _rectT.offsetMax == Vector2.zero)
                return false;
            else
                return true;
        }

        void LinkDataToComponent(FlexibleUIComponent _component, FlexibleUIData _data, LayoutOrientation _orientation)
        {
            if(_orientation == LayoutOrientation.Horizontal)
                _component.HorizontalUIObjectData = _data;
            else
                _component.VerticalUIObjectData = _data;
        }

        void SetFillParentAnchors(FlexibleUIComponent _component)
        {
            _component.FillParentAnchors = true;
        }

        FlexibleUIData CreateFlexibleDataAsset(string _objectName, LayoutOrientation _orientation)
        {
            FlexibleUIData asset = null;
            #if UNITY_EDITOR
                asset = ScriptableObject.CreateInstance<FlexibleUIData>();

                string assetName = _objectName + _orientation + "Data.asset";
                assetName = assetName.Replace(" ", string.Empty);
                asset.name = assetName;
                string completePath = AssetDatabase.GenerateUniqueAssetPath(Path + assetName);

                AssetDatabase.CreateAsset(asset, completePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            #endif
            return asset;
        }

        FlexibleUIData SetFlexibleDataAsset(RectTransform _rectT, FlexibleUIData _asset)
        {
            _asset.RectTranformValues.AnchorMin = _rectT.anchorMin;
            _asset.RectTranformValues.AnchorMax = _rectT.anchorMax;
            _asset.RectTranformValues.OffsetMin = _rectT.offsetMin;
            _asset.RectTranformValues.OffsetMax = _rectT.offsetMax;
            return _asset;
        }

        FlexibleUIData SetFlexibleDataAsset(FlexibleUIData _asset, Vector2 _anchorMin, Vector2 _anchorMax, Vector2 _offsetMin, Vector2 _offsetMax)
        {
            _asset.RectTranformValues.AnchorMin = _anchorMin;
            _asset.RectTranformValues.AnchorMax = _anchorMax;
            _asset.RectTranformValues.OffsetMin = _offsetMin;
            _asset.RectTranformValues.OffsetMax = _offsetMax;
            return _asset;
        }

        FlexibleUIComponent AddFlexibleUIComponent(RectTransform _rectT)
        {
            return _rectT.gameObject.AddComponent<FlexibleUIComponent>();
        }

        public enum LayoutOrientation { Vertical = 0, Horizontal = 1 }
    }
}