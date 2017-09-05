using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AttTypeDefine;
using Assets.Scripts.Helpers;
using Assets.Scripts.ConsoleController.Console;
public class DisabledContainer : MonoBehaviour {

    public static Transform root;

    class CSpawnController
    {
        public Dictionary<int, List<Transform>> dPoolContainer;
        public CSpawnController()
        {
            dPoolContainer = new Dictionary<int, List<Transform>>();
        }
    }

    static CSpawnController spawnController = new CSpawnController();

    void OnEnable ()
    {
        root = transform;
    }

    void OnDisable ()
    {
        root = null;
        spawnController = null;
    }



    public static void AddToDisabledPool (string name, Transform t)
    {

        int hash = NameHashHelper.StringToHash(name);
        List<Transform> lt = null;
        if ((spawnController.dPoolContainer.ContainsKey(hash)))
        {
            lt = spawnController.dPoolContainer[hash];
        }
        else
        {
           lt = new List<Transform>();
            spawnController.dPoolContainer.Add(hash, lt);
        }

        t.parent = root;
        t.gameObject.SetActive(false);
        lt.Add(t);
     }

    public static Transform GetUsedItem(string name)
    {
        int hash = NameHashHelper.StringToHash(name);
        Transform t = null;
        if(spawnController.dPoolContainer.ContainsKey(hash))
        {
            List<Transform> lt = spawnController.dPoolContainer[hash];
            if (lt.Count > 0)
            {
                t = lt[0];
                lt.Remove(t);
             
            }
        }
        else
        {
            DebugHelper.Warn("General", "Pool error");
        }

        return t;

    }

    //public static void AddToDisabledPool (eDisabledType type, Transform t)
    //{
    //    switch (type)
    //    {
    //        case eDisabledType.eType_BezierSpline:
    //            {
    //                if(!lSpline.Contains(t))
    //                {
    //                    t.parent = root;
    //                    t.gameObject.SetActive(false);
    //                    lSpline.Add(t);
    //                }
    //                break;
    //            }
    //        case eDisabledType.eType_FishItem:
    //            {
    //                if (!lFish.Contains(t))
    //                {
    //                    t.parent = root;
    //                    t.gameObject.SetActive(false);
    //                    lFish.Add(t);
    //                }
    //                break;
    //            }
    //        case eDisabledType.eType_DirObj:
    //            {
    //                if (!lDirObj.Contains(t))
    //                {
    //                    t.parent = root;
    //                    t.gameObject.SetActive(false);
    //                    lDirObj.Add(t);
    //                }
    //                break;
    //            }
    //    }
    //}

    //public static Transform GetUsedItem(eDisabledType type)
    //{
    //    Transform t = null;
    //    switch (type)
    //    {
    //        case eDisabledType.eType_DirObj:
    //            {
    //                if (lDirObj.Count > 0)
    //                {
    //                    t = lDirObj[0];
    //                    lDirObj.Remove(t);
    //                }
    //                break;
    //            }
    //        case eDisabledType.eType_BezierSpline:
    //            {
    //                if(lSpline.Count > 0)
    //                {
    //                    t = lSpline[0];
    //                    lSpline.Remove(t);
    //                }
    //                break;
    //            }
    //        case eDisabledType.eType_FishItem:
    //            {
    //                if (lFish.Count > 0)
    //                {
    //                    t = lFish[0];
    //                    lFish.Remove(t);
    //                }
    //                break;
    //            }
    //    }
    //    return t;
    //}



}
