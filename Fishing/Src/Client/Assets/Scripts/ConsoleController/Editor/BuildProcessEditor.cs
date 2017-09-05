using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
namespace Assets.Scripts.ConsoleController.Console.Editor
{
    static class BuildProcessEditor
    {
        [PostProcessSceneAttribute]
        public static void OnPostprocessScene ()
        {
            //如果编辑器在运行状态，则直接返回
            if (EditorApplication.isPlaying)
            {
                return;
            }
            else
            {
                var debugConsole = UnityEngine.Object.FindObjectsOfType<DebugConsole>();
                if (debugConsole.Length > 1)
                {
                    throw new InvalidDataException("More than one debug console in the scene");

                }

                if (debugConsole.Length == 0)
                    return;

              
                //如果用户设置为非Development Mode
                if (!EditorUserBuildSettings.development)
                {
                    UnityEngine.Object.DestroyImmediate(debugConsole[0].gameObject);
                }

                //自动设置版本号
            }


        }
    }
}