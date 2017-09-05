
using UnityEditor;
using UnityEngine;
using Assets.Scripts.ConsoleController.Console.Editor;
[InitializeOnLoad]
class AutomaticInstantiate
{
    static AutomaticInstantiate()
    {
        EditorApplication.playmodeStateChanged += PlaymodeStateChanged;
    }

    private static void PlaymodeStateChanged ()
    {

        if (EditorApplication.isPlaying)
        {
            if (!ConsoleEditorSetting.bAutomaticLoadConsoleInEditor)
                return;

            //Debug.Log("EditorApplication is playing");
            DebugConsole _dc = UnityEngine.Object.FindObjectOfType<DebugConsole>();
            if (_dc == null)
            {
                //Debug.Log("DebugConsole is null");
                var prefab = AssetDatabase.LoadMainAssetAtPath("Assets/Resources/ConsoleController/Prefabs/DebugConsole.prefab");
                if (prefab == null)
                {
                    //Debug.LogWarning("Couldn't load DebugConsole as the DebugConsole prefab couldn't be found at " + debugConsolePath + ". If you have moved the OpenCoding folder, please update the location in DebugConsoleEditorSettings.");
                    return;
                }
                var go = UnityEngine.Object.Instantiate(prefab) as GameObject;
                go.name = "DebugConsole (Automatically Instantiated)";

            }
        }
        //else
        //{
        //    //Debug.Log("EditorApplication isn't playing");
        //}


    }

    //private static GameObject Instantiate(UnityEngine.Object prefab)
    //{
    //    throw new NotImplementedException();
    //}
}


