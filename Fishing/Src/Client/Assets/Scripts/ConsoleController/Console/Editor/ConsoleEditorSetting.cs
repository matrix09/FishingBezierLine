namespace Assets.Scripts.ConsoleController.Console.Editor
{
    using UnityEngine;
    using UnityEditor;
    internal static class ConsoleEditorSetting 
    {
        public static bool bAutomaticLoadConsoleInEditor = false;
        private static readonly string EditorName = "DebugConsoleV1/AutomaticLoadConsoleInEditor";

        static ConsoleEditorSetting () {
            bAutomaticLoadConsoleInEditor = EditorPrefs.GetBool(EditorName, true);
        }

        [PreferenceItem("DebugConsoleV1")]
        private static void SettingsOnGUI()
        {
            bool bValue = GUILayout.Toggle(bAutomaticLoadConsoleInEditor, "Automatically load in editor");
            if(bValue != bAutomaticLoadConsoleInEditor)
            {
                bAutomaticLoadConsoleInEditor = bValue;

                EditorPrefs.SetBool(EditorName, bAutomaticLoadConsoleInEditor);

            }

            GUILayout.Label("This just automatically place DebugConsole Prefab in your scene in the editor mode, which has no effect on distribution versions of your game. ", "helpbox");

        }

    }
}

