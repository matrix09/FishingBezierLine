using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
public class StartUp : MonoBehaviour {

	[HideInInspector]
	public string[] ObjToDestroy = new string[]{ "ManagersDestroy", "__ManagersNotDestroy__"};
	void OnEnable () {

		CheckSceneObj ();
        LoadPrefab("UI/Prefabs/UI Root(3D)", "UI Root(3D)");
		LoadPrefab ("Prefabs/Localization", "Localization");
        Helper.Manager<UIManager>().OpenUISceneByName("UIScene_DBUpdate");
        Invoke("SelfDestroy", 0.5f);
    }



    //check whether 'ManagersDestroy existed'
    //check whether '__ManagersNotDestroy__'
    private void CheckSceneObj () {
		for(int i = 0; i < ObjToDestroy.Length; i++) {

			GameObject obj = GameObject.FindGameObjectWithTag (ObjToDestroy[i]);
			if (null != obj) {
				Destroy (obj);
				obj = null;
			}
		}
	}
	//route : prefab route
	private void LoadPrefab (string route, string prefabName) {
	
		if (null != GameObject.Find (prefabName)) {
			return;
		}
			
		Object obj = Resources.Load (route);

		GameObject gameObj =  Instantiate(obj) as GameObject;    

		gameObj.name = obj.name;
	
	}
    private void SelfDestroy()
    {
        Destroy(gameObject);
    }

}
