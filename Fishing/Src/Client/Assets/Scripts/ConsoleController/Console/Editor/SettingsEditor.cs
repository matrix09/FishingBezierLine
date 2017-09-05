namespace Assets.Scripts.ConsoleController.Console.Editor
{
    using UnityEngine;
    using System.Collections;
    using UnityEditor;
    using Assets.Scripts.ConsoleController.Console;
    [ExecuteInEditMode]
    [CustomEditor(typeof(Settings))]
    public class SettingsEditor : Editor
    {

        private Settings _target;

        private void OnEnable()
        {
            _target = (Settings)target;
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            EditorGUILayout.Space();
            string OpenAndCloseKeys = EditorGUILayout.TextField("PC控制台开关", _target.OpenAndCloseKeys);
            if (_target.OpenAndCloseKeys != OpenAndCloseKeys)
            {
                _target.OpenAndCloseKeys = OpenAndCloseKeys;
            }

            EditorGUILayout.Space();
            bool PauseGameWhenOpen = EditorGUILayout.Toggle("打开是否暂停游戏", _target.PauseGameWhenOpen);
            if (_target.PauseGameWhenOpen != PauseGameWhenOpen)
            {
                _target.PauseGameWhenOpen = PauseGameWhenOpen;
            }

            EditorGUILayout.Space();
            float fWindowPercent = EditorGUILayout.Slider(
                "控制台缩放比例",
               _target.fWindowPercent,
               0.5f, 1f);
            if (_target.fWindowPercent != fWindowPercent)
            {
                _target.fWindowPercent = fWindowPercent;
                EditorUtility.SetDirty(_target);
            }

            bool bDontDestroy = EditorGUILayout.Toggle("Don't Destroy", _target.bDontDestroy);
            if (_target.bDontDestroy != bDontDestroy)
            {
                _target.bDontDestroy = bDontDestroy;
            }


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("真机打开控制台");

            int nValue = 0;
            nValue = EditorGUILayout.IntPopup(
                (int)_target._eSelectTouchDetector,
                AssetSettingEditor.strConsoleGesture,
                AssetSettingEditor.nOpenConsoleGesture);

            if (nValue != (int)_target._eSelectTouchDetector)
            {
                _target._eSelectTouchDetector = (Settings.SelectedTouchDetector)nValue;
            }

            EditorGUILayout.EndHorizontal();


        }
    }


}


