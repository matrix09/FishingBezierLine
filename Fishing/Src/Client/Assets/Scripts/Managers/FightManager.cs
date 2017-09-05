using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Assets.Scripts.ConsoleController.Console;
public class FightManager : MonoBehaviour {

    private BaseActor major;
    public BaseActor Major
    {
        get
        {
            return major;
        }
        set
        {
            if(major != value)
            {
                major = value;
            }
        }
    }

    private BaseActor opponent;
    public BaseActor Opponent
    {
        get
        {
            return opponent;
        }
        set
        {
            if (opponent != value)
            {
                opponent = value;
            }
        }
    }

    //场景中，所有NPC的集合
    public Dictionary<GameObject, BaseActor> dTargetDic = new Dictionary<GameObject, BaseActor>();

    private UIScene_FightUI fightui;
    public UIScene_FightUI FightUI
    {
        get
        {
            if(fightui == null)
            {
                GameObject obj = Helper.Manager<UIManager>().OpenUISceneByName("UIScene_FightUI");
                if(null != obj)
                {
                    fightui = obj.GetComponent<UIScene_FightUI>();
                }
            }
            return fightui;
        }
    }
    void OnDisable ()
    {
        dTargetDic.Clear();
        Major = null;
        fightui = null;
    }

    void OnEnable ()
    {
    }

    private RaycastHit hit;
    private float rayLength = 1f;
    Vector3 pos;
    int layer = 0;
    public bool CheckCollider (GameObject obj, Vector3 v)
    {
        bool isvalid = true;

         pos = new Vector3(
            obj.transform.position.x,
            obj.transform.position.y,
            obj.transform.position.z
            );

        pos += new Vector3(v.x, 1f, v.y);
        
        layer = 1 << LayerMask.NameToLayer("Wall");
        if (Physics.Raycast(pos, Vector3.up, out hit, rayLength, layer))
        {
            Debug.DrawLine(pos, hit.point);
            isvalid = false;
        }
        else if (Physics.Raycast(pos, Vector3.right, out hit, rayLength, layer))
        {
            Debug.DrawLine(pos, hit.point);
            isvalid = false;
        }
        else if (Physics.Raycast(pos, Vector3.forward, out hit, rayLength, layer))
        {
            Debug.DrawLine(pos, hit.point);
            isvalid = false;
        }
        return isvalid;
    }


}
