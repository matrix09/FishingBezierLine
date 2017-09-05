using UnityEngine;
using UnityEditor;
using System;
using Assets.Scripts.ConsoleController.Console;
public class Image_AssetEditor : EditorWindow
{


    public static ImageFilesContainer _data;
    // 窗口的显示
    [MenuItem("Window/Image_AssetEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(Image_AssetEditor), false, "Image_AssetEditor");
    }

    string DataPath
    {
        get { return "Assets/Resources/Settings/" + typeof(Image_AssetEditor).FullName + ".asset"; }
    }

    void OnGUI()
    {

        //创建或者加载Assets文件
        if (null == _data)
        {
            _data = AssetDatabase.LoadAssetAtPath(DataPath, typeof(ImageFilesContainer)) as ImageFilesContainer;

            if (null == _data)
            {
                _data = CreateInstance<ImageFilesContainer>() as ImageFilesContainer;
                AssetDatabase.CreateAsset(_data, DataPath);
                AssetDatabase.Refresh();
            }
        }
        //Close
       
        if (GUILayout.Button("提交数据", GUILayout.Width(80)))
        {
            //数据提交
            ScriptableObject scriptable = AssetDatabase.LoadAssetAtPath(DataPath, typeof(ScriptableObject)) as ScriptableObject;
            EditorUtility.SetDirty(scriptable);
            Close();
        }


        //初始化数据

        _data._ActiveGradient = EditorGUILayout.ObjectField("_ActiveGradient", _data._ActiveGradient, typeof(Texture2D), true) as Texture2D;
        _data._AssertIcon = EditorGUILayout.ObjectField("_AssertIcon", _data._AssertIcon, typeof(Texture2D), true) as Texture2D;
        _data._BackgroundGradient = EditorGUILayout.ObjectField("_BackgroundGradient", _data._BackgroundGradient, typeof(Texture2D), true) as Texture2D;
        _data._CheckboxChecked = EditorGUILayout.ObjectField("_CheckboxChecked", _data._CheckboxChecked, typeof(Texture2D), true) as Texture2D;
        _data._CheckboxUnchecked = EditorGUILayout.ObjectField("_CheckboxUnchecked", _data._CheckboxUnchecked, typeof(Texture2D), true) as Texture2D;
        _data._ClearIcon = EditorGUILayout.ObjectField("_ClearIcon", _data._ClearIcon, typeof(Texture2D), true) as Texture2D;
        _data._CloseButton = EditorGUILayout.ObjectField("_CloseButton", _data._CloseButton, typeof(Texture2D), true) as Texture2D;
        _data._CloseKeyboardIcon = EditorGUILayout.ObjectField("_CloseKeyboardIcon", _data._CloseKeyboardIcon, typeof(Texture2D), true) as Texture2D;
        _data._ConsoleInputIcon = EditorGUILayout.ObjectField("_ConsoleInputIcon", _data._ConsoleInputIcon, typeof(Texture2D), true) as Texture2D;
        _data._ErrorIcon = EditorGUILayout.ObjectField("_ErrorIcon", _data._ErrorIcon, typeof(Texture2D), true) as Texture2D;
        _data._ExceptionIcon = EditorGUILayout.ObjectField("_ExceptionIcon", _data._ExceptionIcon, typeof(Texture2D), true) as Texture2D;
        _data._FilterActiveIcon = EditorGUILayout.ObjectField("_FilterActiveIcon", _data._FilterActiveIcon, typeof(Texture2D), true) as Texture2D;
        _data._HelpIcon = EditorGUILayout.ObjectField("_HelpIcon", _data._HelpIcon, typeof(Texture2D), true) as Texture2D;
        _data._HighlightGradient = EditorGUILayout.ObjectField("_HighlightGradient", _data._HighlightGradient, typeof(Texture2D), true) as Texture2D;
        _data._InfoIcon = EditorGUILayout.ObjectField("_InfoIcon", _data._InfoIcon, typeof(Texture2D), true) as Texture2D;
        _data._MaximizeIcon = EditorGUILayout.ObjectField("_MaximizeIcon", _data._MaximizeIcon, typeof(Texture2D), true) as Texture2D;
        _data._MinimizeTopIcon = EditorGUILayout.ObjectField("_MinimizeTopIcon", _data._MinimizeTopIcon, typeof(Texture2D), true) as Texture2D;
        _data._NextHistoryItemIcon = EditorGUILayout.ObjectField("_NextHistoryItemIcon", _data._NextHistoryItemIcon, typeof(Texture2D), true) as Texture2D;
        _data._PreviousHistoryItemIcon = EditorGUILayout.ObjectField("_PreviousHistoryItemIcon", _data._PreviousHistoryItemIcon, typeof(Texture2D), true) as Texture2D;
        _data._RedBackgroundGradient = EditorGUILayout.ObjectField("_RedBackgroundGradient", _data._RedBackgroundGradient, typeof(Texture2D), true) as Texture2D;
        _data._RunIcon = EditorGUILayout.ObjectField("_RunIcon", _data._RunIcon, typeof(Texture2D), true) as Texture2D;
        _data._ScrollbarThumb = EditorGUILayout.ObjectField("_ScrollbarThumb", _data._ScrollbarThumb, typeof(Texture2D), true) as Texture2D;
        _data._SearchIcon = EditorGUILayout.ObjectField("_SearchIcon", _data._SearchIcon, typeof(Texture2D), true) as Texture2D;
        _data._SettingsIcon = EditorGUILayout.ObjectField("_SettingsIcon", _data._SettingsIcon, typeof(Texture2D), true) as Texture2D;
        _data._SettingsPopupBackground = EditorGUILayout.ObjectField("_SettingsPopupBackground", _data._SettingsPopupBackground, typeof(Texture2D), true) as Texture2D;
        _data._SuggestionButtonBackground = EditorGUILayout.ObjectField("_SuggestionButtonBackground", _data._SuggestionButtonBackground, typeof(Texture2D), true) as Texture2D;
        _data._WarningIcon = EditorGUILayout.ObjectField("_WarningIcon", _data._WarningIcon, typeof(Texture2D), true) as Texture2D;
        _data._clean = EditorGUILayout.ObjectField("_CleanIcon", _data._clean, typeof(Texture2D), true) as Texture2D;


       



    }

}
