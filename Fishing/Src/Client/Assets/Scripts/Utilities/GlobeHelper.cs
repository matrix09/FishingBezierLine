using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using AttTypeDefine;
using Assets.Scripts.Utilities;
using Assets.Scripts.Managers.Attributes;
using Assets.Scripts.ConsoleController.Console;
using Assets.Scripts.DataStore;
using Assets.Scripts.Attributes;
using Assets.Scripts.Managers.Controller;
public class GlobeHelper : MonoBehaviour {

    #region 常量
    public static class Constants
    {
        public const string FISH_01 = "Prefabs/Fish/Fish_01";
        public const string ENEMY = "Enemy";
        public const string NPC = "Npc";
        public const string PLAYER = "Player";
        public const string PET = "Pet";
        public const string HERO = "Hero";
        public const string Guard = "Guard";

    }
    #endregion

    #region 通用接口
   

    public static float GetCurrentTime (bool ignore)
    {
        float cur = 0f;

        cur = ignore == true ? Time.realtimeSinceStartup : Time.time;

        return cur;
    }

    public static float GetDeltaTime(bool isIgnoreTimeScale)
    {
        return (isIgnoreTimeScale) ? Time.unscaledDeltaTime : Time.deltaTime;
    }


    public static void SetGameObjectLayer (GameObject obj, int layer)
    {
        obj.layer = layer;
        for(int i = 0; i < obj.transform.childCount; i++)
        {
            obj.transform.GetChild(i).gameObject.layer = layer;
            GameObject g = obj.transform.GetChild(i).gameObject;
            if(g.transform.childCount > 0)
            {
                SetGameObjectLayer(g, layer);
            }
        }
    }

    public static void SetGameObjectLayer (GameObject obj, string layername)
    {
        int layer = LayerMask.NameToLayer(layername);
        SetGameObjectLayer(obj, layer);
    }

    public static string LocaTempToStr(int id)
    {
        string str = "none";

        using (SystemDataStore.DataRecord record =
           SystemDataStore.Instance.FetchRecord<LocalizeTemplate>(id))
        {
            while (record.Read())
            {
                str = record.GetString(LocalizeTemplate.Description);
            }
        }
        return str;
    }

    // 获得本地当前DateTime
    public static DateTime GetDateTimeNow()
    {
        try
        {
            return DateTime.Now;
        }
        catch (Exception)
        {
            return DateTime.Now;
        }
    }

    public static List<T> GetComponentsAll<T>(GameObject go, bool includeInactive) where T : Component
    {
        List<T> listT = new List<T>();
        if (go != null)
        {
            T t = go.GetComponent<T>();
            if (t != null)
            {
                listT.Add(t);
            }
            T[] ts = go.GetComponentsInChildren<T>(includeInactive);
            if (ts != null)
            {
                for (int i = 0, max = ts.Length; i < max; i++)
                {
                    T tt = ts[i];
                    listT.Add(tt);
                }
            }
        }
        return listT;
    }
    #endregion

    #region 属性变量
    public  class CurrentSceneLoader
    {
        
        public FightManager FightMgr;
    }

    private static CurrentSceneLoader curSceneLoader =  new CurrentSceneLoader();
    public static CurrentSceneLoader CurSceneLoader
    {
        get
        {
            return curSceneLoader;
        }
        set
        {
            if(value != curSceneLoader)
            {
                curSceneLoader = value;
            }
        }
    }
    #endregion

    #region 实例对象通用接口
    static private BeizierSpline CreateSpline()
    {
        //回收站获取曲线实例
        // Transform t = DisabledContainer.GetUsedItem(eDisabledType.eType_BezierSpline);
        GameObject obj = null;
        InitializeGameObject("Prefabs/Fish/Splines/BeizierSpline", SplineContainer.root, "BeizierSpline", ref obj);
        return obj.GetOrAddComponent<BeizierSpline>();
    }
    public static void InitializeGameObject(string route, Transform parent, string name, ref GameObject o)
    {
        GameObject obj;

        if (o == null)
        {
            Transform t = DisabledContainer.GetUsedItem(name);

            if (null == t)
            {
                UnityEngine.Object _obj = Resources.Load(route);
                obj = Instantiate(_obj) as GameObject;
                obj.name = _obj.name;
                t = obj.transform;
            }

            o = t.gameObject;
        }

        o.transform.parent = parent;
        o.transform.localEulerAngles = (Vector3.zero);
        o.transform.localPosition = Vector3.zero;
        o.transform.localScale = Vector3.one;

        if (!o.activeInHierarchy && !o.activeSelf)
        {
            o.SetActive(true);
        }

    }

    #endregion

    #region 创建角色

    public static BaseActor CreateMajor(int id, Vector3 pos, Vector3 rot, float scale = 1f)
    {

        PlayerBase nb = DataRecordManager.TempBase<PlayerBase>(id);

        //在回收站里面去查找对象 
        Transform t = DisabledContainer.GetUsedItem(nb.RoleName);
        GameObject FrameObj = null;
        //实例GameObject对象，将指定模型放在GameObject下面
        if (t == null)
        {
            FrameObj = new GameObject();
        }
        else
        {
            FrameObj = t.gameObject;
            if(!FrameObj.activeSelf && !FrameObj.activeInHierarchy)
            {
                FrameObj.SetActive(true);
            }
        }
        FrameObj.transform.parent = null;
        FrameObj.transform.localRotation = Quaternion.Euler(rot);
        FrameObj.transform.localPosition = pos;
        FrameObj.transform.localScale = new Vector3(scale, scale, scale);
        FrameObj.tag = Constants.PLAYER;

        BaseActor ba = FrameObj.GetOrAddComponent<BaseActor>();
        ba.CreateBaseActor(nb.ModelPath, nb.RoleName);
         
        SetGameObjectLayer(FrameObj, Constants.PLAYER);

        //读取属性表, 设置属性
        ba.GetOrAddComponent<PlayerAttribute>();
        
        ba.GetOrAddComponent<PlayerControl>();

        //设置阵营
        ba.BaseAtt.PlayerType = ePlayerType.eType_Player;
        ba.BaseAtt.RelativePlayerType = eRelativePlayerType.eType_Friend;

        return ba;
    }

    public static BaseActor CreateEnemy(int id, Vector3 pos, Vector3 rot, float scale = 1f)
    {

        PlayerBase nb = DataRecordManager.TempBase<PlayerBase>(id);

        //在回收站里面去查找对象 
        Transform t = DisabledContainer.GetUsedItem(nb.RoleName);
        GameObject FrameObj = null;
        //实例GameObject对象，将指定模型放在GameObject下面
        if (t == null)
        {
            FrameObj = new GameObject();
        }
        else
        {
            FrameObj = t.gameObject;
            if (!FrameObj.activeSelf && !FrameObj.activeInHierarchy)
            {
                FrameObj.SetActive(true);
            }
        }
        FrameObj.transform.parent = null;
        FrameObj.transform.localRotation = Quaternion.Euler(rot);
        FrameObj.transform.localPosition = pos;
        FrameObj.transform.localScale = new Vector3(scale, scale, scale);
        FrameObj.tag = Constants.ENEMY;

        BaseActor ba = FrameObj.GetOrAddComponent<BaseActor>();
        ba.CreateBaseActor(nb.ModelPath, nb.RoleName);

        SetGameObjectLayer(FrameObj, Constants.ENEMY);

        //读取属性表, 设置属性
        ba.GetOrAddComponent<PlayerAttribute>();

        //ba.GetOrAddComponent<PlayerControl>();

        //设置阵营
        ba.BaseAtt.PlayerType = ePlayerType.eType_Enemy;
        ba.BaseAtt.RelativePlayerType = eRelativePlayerType.eType_Enemy;

        return ba;
    }

    ////加载BaseActor接口，将BaseActor脚本放在GameObject下面
    //BaseActor ba = FrameObj.GetOrAddComponent<BaseActor>();
    //ba.actor = modelObj;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">角色模板id</param>
    /// <param name="splineId">角色曲线模板id</param>
    /// <param name="isTrig">是否马上触发移动</param>
    /// <returns></returns>
    public static BaseActor CreateNpc (int id, int splineId, float scale = 1f, bool isTrig = true)
    {
        NpcBase nb = DataRecordManager.TempBase<NpcBase>(id);

        //在回收站里面去查找对象 
        Transform t = DisabledContainer.GetUsedItem(nb.RoleName);
        GameObject FrameObj = null;
        //实例GameObject对象，将指定模型放在GameObject下面
        if (t == null)
        {
            FrameObj = new GameObject();
        }
        else
        {
            FrameObj = t.gameObject;
            if (!FrameObj.activeSelf && !FrameObj.activeInHierarchy)
            {
                FrameObj.SetActive(true);
            }
        }
        FrameObj.transform.parent = NpcContainer.root;
        FrameObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        FrameObj.transform.localPosition = Vector3.zero;
        FrameObj.transform.localScale = new Vector3(scale, scale, scale);
        FrameObj.tag = Constants.NPC;
      
        BaseActor ba = FrameObj.AddComponent<BaseActor>();
        ba.CreateBaseActor(nb.ModelPath, nb.RoleName);

        SetGameObjectLayer(FrameObj, Constants.NPC);

        //读取属性表, 设置属性
        ba.GetOrAddComponent<NpcAttribute>();

        ba.GetOrAddComponent<NpcControl>();

        //设置阵营 
        ba.BaseAtt.PlayerType = ePlayerType.eType_NPC;
        ba.BaseAtt.RelativePlayerType = eRelativePlayerType.eType_Neutral;

        //加载曲线，设定模型路径
        //创建曲线实例对象,将gameobject加入到splinecontainer
        BeizierSpline bs = CreateSpline();

        SplineWalkerBase swb = DataRecordManager.TempBase<SplineWalkerBase>(splineId);
        //初始化曲线数据
        float fRadius = 0.5f;
        bs.SetSplineData(swb, fRadius);

        ba.BS = bs;

        //fi是否开始运动
        ba.BTrigMove = isTrig;

        return ba;
    }




    #endregion

}
