using UnityEngine;
using System.Collections.Generic;
namespace AttTypeDefine {
    #region Bezier曲线
    public enum BezierControlPointMode
    {
        Free,
        Aligned,
        Mirrored,
    }

    public enum eBirthSide
    {
        Start = 0,
        Left = 0,
        Right = 1,
        Top = 2,
        Bottom = 3,
        End = 3,
    }

    #endregion


    public enum ePlayerType
    {
        eType_Player,//玩家
        eType_NPC,//NPC
        eType_Enemy,//敌人
    }

    public enum eRelativePlayerType
    {
        eType_Friend,
        eType_Neutral,
        eType_Enemy,
    }

    //基础属性 
    public enum eBasicAttribute
    {
        eType_Attack,//攻击
        eType_Defence,//防御
        eType_HP,//血量
        eType_MP,//蓝量
    }

    //相机类型

    public enum eCamType
    {
        eType_Follow,

    }

    


}

