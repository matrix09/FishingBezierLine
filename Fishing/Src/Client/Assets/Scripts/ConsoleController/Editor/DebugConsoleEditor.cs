using UnityEditor;
using UnityEngine;
using System.IO;
namespace Assets.Scripts.ConsoleController.Console.Editor
{
    [CustomEditor(typeof(DebugConsole))]
    public class DebugConsoleEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            DebugConsole myTarget = (DebugConsole)target;
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Settings..."))
            {

                if(myTarget.Setting == null)
                {
                    myTarget.Setting = (Resources.Load("ConsoleController/Settings/Setting_AssetEditor")) as Settings;
                }

                Selection.activeObject = myTarget.Setting;
            }

            GUILayout.EndHorizontal();
        }

    }
}