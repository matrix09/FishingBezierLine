using UnityEngine;
using System.Collections;
using Assets.Scripts.Modules;
using Assets.Scripts.Utilities;
using Assets.Scripts.Attributes;
using Assets.Scripts.ConsoleController.Console;
using Assets.Scripts.Managers.Skill;
public class BaseActor : MonoBehaviour {
    [HideInInspector]
    public GameObject actor;

    private AnimatorManager animmgr;
    public AnimatorManager AnimMgr
    {
        get
        {
            if(null == animmgr)
            {
                animmgr = actor.GetOrAddComponent<AnimatorManager>();
            }

            return animmgr;
        }
    }

    private BaseAttribute baseAtt;
    public BaseAttribute BaseAtt
    {
        get
        {
            if(null == baseAtt)
            {
                baseAtt = gameObject.GetOrAddComponent<BaseAttribute>();
            }

            return baseAtt;
        }
    }

    private Animator animator;
    public Animator Anim
    {
        get
        {
            if (animator == null)
            {
                animator = actor.GetComponent<Animator>();
            }
            return animator;
        }
    }

    private FSMBehaviour fsm;
    public FSMBehaviour FSMB
    {
        get
        {
            if(null == fsm)
            {
                fsm = gameObject.GetOrAddComponent<FSMBehaviour>();
            }
            return fsm;
        }
    }

    private SkillManager skillMgr;
    public SkillManager SkillMgr
    {
        get
        {
            if(skillMgr == null)
            {
                skillMgr = gameObject.GetOrAddComponent<SkillManager>();
                skillMgr.Owner = this;
            }

            return skillMgr;

        }
    }

    



    public BaseActor CreateBaseActor(string path, string _name)
    {
        if (null == path)
        {
            this.Error("General", "CreateBaseActor : path == null");
            return null;
        }
        GameObject modelObj = null; Object obj = null;
        //加载指定路径模型
        if (gameObject.transform.childCount == 1)
        {
            //不需要重新实例化
            modelObj = transform.GetChild(0).gameObject;
        }
        else if(gameObject.transform.childCount == 0)
        {
            obj = Resources.Load(path);
            modelObj = Instantiate(obj) as GameObject;
            modelObj.transform.parent = gameObject.transform;
            modelObj.name = obj.name;
        }
        else
        {
            this.Error("General", "error logic for createbaseactor");
        }
       
       
        modelObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        modelObj.transform.localPosition = Vector3.zero;
        modelObj.transform.localScale = Vector3.one;
        name = _name;
        actor = modelObj;

        if (null == AnimMgr ||
           null == BaseAtt ||
           null == Anim ||
           null == FSMB
           )
        {
            DebugHelper.Error("General", "Logic Error");
            return null;
        }

        ResetAllData();

        return this;
    }
    
    void ResetAllData ()
    {
        BTrigMove = false;
        m_progress = 0f;
    }

    #region 曲线实例,属性
    private BeizierSpline bs;
    public BeizierSpline BS
    {
        get
        {
            return bs;
        }
        set
        {
            bs = value;
        }
    }


    #endregion

    private bool btrig = false;
    public bool BTrigMove
    {
        get
        {
            return btrig;
        }
        set
        {
            if (btrig != value)
            {
                //目标回收
                if (btrig == true && value == false)
                {
                    if(m_progress == 1f)
                    {
                        //播放鱼的死亡特效

                        RetriveNpc( bs.gameObject.transform);
                    }
                    //受到指定技能，而导致无法移动 
                    else
                    {
                        
                    }
                }
                //鱼开始运动
                else if (btrig == false && value == true)
                {
                    AddNpc();
                }

                btrig = value;
            }
        }
    }

    public void TerminateRoute ()
    {
        m_progress = 1f;
        BTrigMove = false;
    }

    float m_progress = 0f;
    Vector3 vCurPos;
    Vector3 vNextPos;
    Vector3 vTmp = Vector3.zero;
    void Update()
    {
        if(BTrigMove)
        {
            m_progress += bs.GetDt(m_progress);
            if(m_progress < 1f)
            {
                vCurPos = bs.GetPoint(m_progress);

                vTmp = new Vector3(
                        (bs.GetDirection(m_progress) * 0.5f).x,
                        (bs.GetDirection(m_progress) * 0.5f).z,
                        0
                    );

                if (GlobeHelper.CurSceneLoader.FightMgr.CheckCollider(gameObject, vTmp))
                {
                    transform.position = new Vector3(
                                    vCurPos.x,
                                    transform.position.y,
                                    vCurPos.z
                    );
                }
                else
                {//没走完，撞墙了，走到了边界，那么需要重新规划运动轨迹，且前两个点的运动方向和之前的运动轨迹反向
                    ResetPathPoint(BeizierSpline.eCheckHitWall.eType_HitWall, m_progress);
                    return;
                }
                
                vNextPos = vCurPos + bs.GetDirection(m_progress) * 0.5f;
                transform.LookAt(vNextPos);
            }
            else
            {
                //progress = 0f;
                //BTrigMove = false;
                //vTmp = new Vector3(
                //        (bs.GetDirection(1f) * 0.5f).x,
                //        (bs.GetDirection(1f) * 0.5f).z,
                //        0
                //    );

                vTmp = Vector3.one;

                //走完了， 没有撞墙
                if (GlobeHelper.CurSceneLoader.FightMgr.CheckCollider(gameObject, vTmp))
                {
                    ResetPathPoint(BeizierSpline.eCheckHitWall.eType_NotHitWall, 1f);
                }
                //走完了，撞墙了 
                else
                {
                    ResetPathPoint(BeizierSpline.eCheckHitWall.eType_HitWall, 1f);
                }
            }
        }   
    }

    void AddNpc ()
    {
        transform.position = BS.GetPoint(0);

        if(!GlobeHelper.CurSceneLoader.FightMgr.dTargetDic.ContainsKey(gameObject))
        {
            GlobeHelper.CurSceneLoader.FightMgr.dTargetDic.Add(gameObject, this);
        }
        else
        {
            this.Error("General", "error logic");
        }

    }

    void RetriveNpc (Transform t)
    {
        if (GlobeHelper.CurSceneLoader.FightMgr.dTargetDic.ContainsKey(gameObject))
        {
            GlobeHelper.CurSceneLoader.FightMgr.dTargetDic.Remove(gameObject);
            this.LogFormat("General", "count = {0}", GlobeHelper.CurSceneLoader.FightMgr.dTargetDic.Count);
        }
        else
        {
            this.Error("General", "error logic");
        }

        //将曲线放到回收站
        BeizierSpline.DisableBeizierSpline(bs.gameObject.transform);
        //将鱼放到回收站
        DisabledContainer.AddToDisabledPool(name, transform);
    }

    //重置点坐标
    public void ResetPathPoint(BeizierSpline.eCheckHitWall type, float _progress)
    {
        Vector3 staPoint = transform.position;

        BS.ResetPathPoint(transform, type, _progress);

        m_progress = 0f;
    }

}




