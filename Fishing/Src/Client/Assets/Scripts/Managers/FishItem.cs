using UnityEngine;
using System.Collections;

public class FishItem : MonoBehaviour {

    #region FishItem 实例
    public GameObject ItemObj;
    #endregion

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
            if(btrig != value)
            {
                //鱼回收
                if(btrig == true && value == false)
                {
                    //播放鱼的死亡特效

                    //将曲线放到回收站
                    BeizierSpline.DisableBeizierSpline(bs.gameObject.transform);
                    //将鱼放到回收站
                    DisabledContainer.AddToDisabledPool(name, transform);
                }
                //鱼开始运动
                else if(btrig == false && value == true)
                {
                    transform.position = BS.GetPoint(0);
                }

                btrig = value;
            }
        }
    }

    float progress = 0f;
    void Update ()
    {
        if (BTrigMove == false) return;

        progress += bs.GetDt(progress);
        if (progress >= 1f)
        {
            BTrigMove = false;
            progress = 0f;
            return;
        }
        transform.position = bs.GetPoint(progress);
    }

}
