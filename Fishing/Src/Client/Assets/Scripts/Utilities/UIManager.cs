using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utilities;
using AttTypeDefine;
using Assets.Scripts.Helpers;

public class UIManager : MonoBehaviour
{
    protected GameObject uiroot;
    protected GameObject uianchor;
    protected GameObject uirootUp;
    protected GameObject uianchorUp;
    private string uiRootStr = "UI Root(3D)";
    private string uiAnchorStr = "AutoScale";

    private string uiPrefabStr = "UI/Prefabs/";

    private Dictionary<string, string> scenceDic;



    private GameObject UIRoot
    {
        get
        {
            if (uiroot == null)
            {
                uiroot = GameObject.Find(uiRootStr);
                if (uiroot == null)
                {
                    GameObject obj = Resources.Load(uiPrefabStr + uiRootStr) as GameObject;
                    if (null != obj)
                    {
                        uiroot = Instantiate(obj) as GameObject;
                    }

                    return null;
                }

            }
            return uiroot;
        }
    }
    private GameObject UIAnchor
    {
        get
        {
            if (null == uianchor)
            {
                uianchor = GameObject.Find(uiAnchorStr);
            }
            return uianchor;
        }

    }


    private GameObject FindSceneByName(string sceneName)
    {


            if (null == UIAnchor)
            {
                return null;
            }

            if (UIAnchor.transform.childCount == 0)
            {
                return null;
            }

            Transform child = UIAnchor.transform.FindChild(sceneName);

            return child == null ? null : child.gameObject;
        
       
    }

    public GameObject OpenUISceneByName(string sceneName, string indexStr = "")//如果true 界面显示在摄像机靠前的ui下面 反之在靠后的
    {
        
        GameObject uiScene = FindSceneByName(sceneName + indexStr);
        if (null != uiScene)
        {
            if (!uiScene.activeInHierarchy)
                uiScene.SetActive(true);
            return uiScene;
        }


            if (UIRoot)
            {
                GameObject obj = Resources.Load(uiPrefabStr + sceneName) as GameObject;
                if (null != obj)
                {
                    uiScene = Instantiate(obj) as GameObject;
                    uiScene.transform.parent = UIAnchor.transform;
                    uiScene.transform.localScale = Vector3.one;
                    uiScene.transform.localPosition = Vector3.zero;
                    uiScene.name = obj.name;
                    uiScene.name = obj.name + indexStr;
                    return uiScene;
                }
            }

        
      
        return null;
    }

    public void CloseUISceneByName(string sceneName)
    {
   
        GameObject UIScene = FindSceneByName(sceneName);
        //UIScene = GameObject.Find(sceneName);

        if (UIScene != null && UIScene.activeInHierarchy)
        {
            UIScene.SetActive(false);
        }
    }


	public void DestroyUISceneByName (string sceneName) {
		GameObject UIScene = FindSceneByName(sceneName);
		if (null != UIScene) {
			Destroy (UIScene);
		}
	}

    //todo
    public void CloseAllUIScene(string ignorSceneName)
    {
       
      
    }

    public void DestroyAllUIScence()
    {
        int count = UIAnchor.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(UIAnchor.transform.GetChild(i).gameObject);
        }
    }


    public T UI<T>() where T : Component
    {
        var scene = FindSceneByName(typeof(T).Name);
        if (scene)
        {
            return scene.GetComponent<T>();
        }

        return null;
    }

    public T UI<T>(string sceneName) where T : Component
    {
        var scene = FindSceneByName(sceneName);
        if (scene)
        {
            return scene.GetComponent<T>();
        }

        return null;
    }

    public T UIAndActive<T>() where T : Component
    {
        var scene = FindSceneByName(typeof(T).Name);
        if (scene)
        {
            T target = scene.GetComponent<T>();
            if (target.gameObject.activeInHierarchy)
            {
                return target;
            }
        }

        return null;
    }

}
