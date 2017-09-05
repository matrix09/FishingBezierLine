using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Helpers {
    public static class Helper {

        static GameObject localPlayer;

        public static T LocalPlayer<T>() where T : Component {
            if ( localPlayer == null ) {
                localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer");
                if ( localPlayer == null ) {
                    return null;
                }
            }

            return localPlayer.GetOrAddComponentInChildren<T>();
        }

        static GameObject globalManager;
        public static readonly string TagManagers = "__ManagersNotDestroy__";

        public static T Manager<T>() where T : Component {

            return Singleton<T>(ref globalManager, TagManagers, false);
        }

        static GameObject globalSingleton;
        public static readonly string TagSingleton = "__SingletonGlobal__";

        public static T Singleton<T>() where T : Component {
            return Singleton<T>(ref globalSingleton, TagSingleton, false);
        }

        public static T Singleton<T>( ref GameObject objSingleton, string inTag, bool destory ) where T : Component {

            if ( objSingleton == null ) {
                objSingleton = GameObject.FindGameObjectWithTag(inTag);
                if ( objSingleton == null ) {
                    objSingleton = new GameObject(inTag) { tag = inTag };
                    if ( !destory ) {
                        Object.DontDestroyOnLoad(objSingleton);
                    }
                }
            }

            return objSingleton.GetOrAddComponent<T>();
        }

        public static bool IsWebplayer() {
            return Application.platform == RuntimePlatform.OSXDashboardPlayer
                   || Application.platform == RuntimePlatform.OSXWebPlayer
                   || Application.platform == RuntimePlatform.WindowsWebPlayer;
        }

        public static bool IsStandalone() {
            return Application.platform == RuntimePlatform.WindowsPlayer
                   || Application.platform == RuntimePlatform.OSXPlayer;
        }

        public static bool IsEditor() {
            return Application.platform == RuntimePlatform.WindowsEditor
                   || Application.platform == RuntimePlatform.OSXEditor;
        }

        public static WWW Download( string url ) {

            var www = new WWW(url);
            while ( !www.isDone ) { }

            return www;
        }
        public static T UIAndActive<T>() where T : Component
        {
            return SceneManager<UIManager>().UIAndActive<T>();
        }
        static GameObject sceneManager;
        public static readonly string TagSceneManager = "ManagersDestroy";
        public static T SceneManager<T>() where T : Component
        {
            return Singleton<T>(ref sceneManager, TagSceneManager, true);
        }
    }
}
