using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FlexibleUI
{
    [System.Serializable]
    [CanEditMultipleObjects]
    [CustomEditor(typeof(FlexibleUIMaker))]
    public class FlexibleUIMakerEditor : Editor
    {
        FlexibleUIMaker flexibleUIMaker;

        private void OnEnable()
        {
            flexibleUIMaker = (FlexibleUIMaker)target;
            if (flexibleUIMaker.Path == null || flexibleUIMaker.Path == string.Empty)
                flexibleUIMaker.CheckFolder();
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Select folder for data"))
            {
                SelectFolder();
            }

            EditorGUILayout.Space();
            flexibleUIMaker.Path = EditorGUILayout.TextField("Data Path", flexibleUIMaker.Path);
            EditorGUILayout.Space();
            flexibleUIMaker.WorkOnlyOnThisObject = EditorGUILayout.Toggle("Work Only On This Object", flexibleUIMaker.WorkOnlyOnThisObject);
            EditorGUILayout.Space();
            flexibleUIMaker.ChangeExistingData = EditorGUILayout.Toggle("Change Existing Data", flexibleUIMaker.ChangeExistingData);
            EditorGUILayout.Space();
            flexibleUIMaker.SelectedLayoutOrientation = (FlexibleUIMaker.LayoutOrientation)EditorGUILayout.EnumPopup("Selected Layout Orientation", flexibleUIMaker.SelectedLayoutOrientation);
            EditorGUILayout.Space();

            if (GUILayout.Button("Make UI"))
            {
                flexibleUIMaker.MakeUI();
            }
        }

        void SelectFolder()
        {
            string fullPath = EditorUtility.OpenFolderPanel("Select folder for data", "Assets", "");
            string tempPath = GetRelativePath(fullPath, '/');

            if (tempPath != string.Empty)
                flexibleUIMaker.Path = tempPath;
            else
                Debug.LogError("Invalid directory !");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        string GetRelativePath(string _string, char _token)
        {
            string relativePath = string.Empty;
            string[] strArr = _string.Split(_token);

            int startIndex = - 1;

            for (int i = 0; i < strArr.Length; i++)
            {
                if (strArr[i].Contains("Assets"))
                {
                    startIndex = i;
                    break;
                }
            }

            if(startIndex >= 0)
            {
                for (int i = startIndex; i < strArr.Length; i++)
                {
                    relativePath += strArr[i] + _token;
                }
            }

            return relativePath;
        }
    }
}