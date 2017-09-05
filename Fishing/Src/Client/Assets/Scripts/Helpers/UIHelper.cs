using UnityEngine;

namespace Assets.Scripts.Helpers {
    public static class UIHelper {

        private static GameObject sceneManager;
        public static readonly string TagSceneManager = "ManagersDestroy";

//         public static T SceneManager<T>() where T : Component {
//             return Helper.Singleton<T>(ref sceneManager, TagSceneManager, true);
//        
        public static T UI<T>() where T : Component {
            return Helper.Manager<UIManager>().UI<T>();
        }

        public static T UI<T>( string sceneName ) where T : Component {
            return Helper.Manager<UIManager>().UI<T>(sceneName);
        }

        public static T UIAndActive<T>() where T : Component {
            return Helper.Manager<UIManager>().UIAndActive<T>();
        }
    }
}
