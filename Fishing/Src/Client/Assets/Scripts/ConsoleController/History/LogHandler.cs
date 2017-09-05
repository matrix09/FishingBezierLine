namespace Assets.Scripts.ConsoleController.History
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class LogHandler
    {
//        private static Application.LogCallback callbacks = new Application.LogCallback(delegate (string condition, string stackTrace, LogType type));

        public static void RegisterLogCallback(Application.LogCallback callback)
        {
            if (Application.unityVersion.StartsWith("4"))
            {
                Debug.LogError("This version of TouchConsole Pro is designed to work with Unity 5 and newer only. Reimport the latest version from the asset store to get the Unity 4 version.");
            }
            Application.logMessageReceived += callback;
        }

        public static void UnRegisterLogCallback(Application.LogCallback callback)
        {
            Application.logMessageReceived -= callback;
        }


        /*
        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly LogHandler.<>c<>9 = new LogHandler.<>c();
        }
        */

        // internal static void InterLogCallBack (string condition, string stacktrack, LogType type)
        //{

        //}
    }
}

