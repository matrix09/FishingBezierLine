using UnityEngine;
using UnityEditor;
namespace Assets.Scripts.ConsoleController.Console.Editor
{
    public class AssetSettingEditor : EditorWindow
    {

        public static Settings _data;

        public static string[] strConsoleGesture = new string[] {"TWO FINGER SWIPE DOWN" };
        public static int[] nOpenConsoleGesture = new int[] { 1 };

        [MenuItem("Window/Setting_AssetEditor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(AssetSettingEditor), false, "Setting_AssetEditor");
        }

        string DataPath
        {
            get
            {
                return "Assets/Resources/ConsoleController/Settings/" + "Setting_AssetEditor" + ".asset";
            }
        }

        void OnGUI()
        {

            //创建或者加载Assets文件
            if (null == _data)
            {
                _data = AssetDatabase.LoadAssetAtPath(DataPath, typeof(Settings)) as Settings;

                if (null == _data)
                {
                    _data = CreateInstance<Settings>() as Settings;
                    AssetDatabase.CreateAsset(_data, DataPath);
                    AssetDatabase.Refresh();
                }
            }

            //keyboard 开关debugconsole
            _data.OpenAndCloseKeys = EditorGUILayout.TextField("打开控制台按键", _data.OpenAndCloseKeys);
            _data.PauseGameWhenOpen = EditorGUILayout.Toggle("是否打开暂停游戏", _data.PauseGameWhenOpen);
            _data.fWindowPercent = EditorGUILayout.Slider(
              "控制台缩放比例",
            _data.fWindowPercent,
             0.5f, 1f);

            _data.bDontDestroy = EditorGUILayout.Toggle("Don't Destroy", _data.bDontDestroy);

            int nValue = 0;
            nValue = EditorGUILayout.IntPopup(
                (int)_data._eSelectTouchDetector,
                strConsoleGesture,
                nOpenConsoleGesture);

            if (nValue != (int)_data._eSelectTouchDetector)
            {
                _data._eSelectTouchDetector = (Settings.SelectedTouchDetector)nValue;
            }


            //Close
            GUI.color = Color.green;
            if (GUILayout.Button("提交数据", GUILayout.Width(80)))
            {
                //数据提交
                ScriptableObject scriptable = AssetDatabase.LoadAssetAtPath(DataPath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty(scriptable);
                Close();
            }











        }



    }

}