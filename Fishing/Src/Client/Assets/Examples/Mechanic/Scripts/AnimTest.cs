using UnityEngine;
using Assets.Scripts.Modules;
using Assets.Scripts.Helpers;
using Assets.Scripts.Managers.Attributes;
using Assets.Scripts.DataStore;
public class AnimTest : MonoBehaviour {

    private void ConnectSystemDataStore()
    {
        SystemDataStore.Instance.Disconnect();
        SystemDataStore.Instance.Connect("GameDataStore.db");
    }

    // Use this for initialization
    void Start () {
      
        //连接数据库
        ConnectSystemDataStore();
     
        //初始化当前场景全局数据
        InitSceneData();
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();

        GUILayoutOption[] optionArray = new GUILayoutOption[] { GUILayout.Width(120), GUILayout.Height(60) };

        if (GUILayout.Button("攻击", optionArray))
        {
            GlobeHelper.CurSceneLoader.FightMgr.Major.SkillMgr.UseSkill(1001);
        }

        GUILayout.EndVertical();
    }

    void AnimClipEnd ()
    {
        
    }
    
    void Update ()
    {
        if(GlobeHelper.CurSceneLoader.FightMgr.dTargetDic.Count < 10)
        {
            launch();
        }
    }

    void InitSceneData ()
    {
        //获取当前场景的战斗管理器
        GlobeHelper.CurSceneLoader.FightMgr = GetComponent<FightManager>();

        //创建Player
        GlobeHelper.CurSceneLoader.FightMgr.Major = GlobeHelper.CreateMajor(10001, Vector3.zero, Vector3.zero, 1);

        //创建enemy
        GlobeHelper.CurSceneLoader.FightMgr.Opponent = GlobeHelper.CreateEnemy(20001, new Vector3(10f, 0f, 10f), Vector3.zero, 1);

    }

    void LaunchFish()
    {
        float t = Random.Range(0.5f, 3f);
        InvokeRepeating("launch", 0, t);
    }

    void launch()
    {
        GlobeHelper.CreateNpc(20001, 10001, 2);
    }

}
