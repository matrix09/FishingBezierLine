namespace Assets.Scripts.DataStore {

    public enum Common {
        AttId
    }

    //参数配置表
    public enum ParamTemplate {
        AttId,
        Param
    }

   

    //zxf 角色出生公共模板属性
    public enum AllTemplate {
        AttId,
        AttNum
    }

    public enum SplineWalkerTemplate
    {
        AttId,
        MinSpeed,
        MaxSpeed,
        MinCameraDistance,
        MaxCameraDistance,
        MinDirValue,
        MaxDirValue,
        MinPointNum,
        MaxPointNum,
        BIsCycle,
        FishRoute
    }


    //NPC模板
    public enum InitNpcTemplate
    {
        AttId,
        RoleId,
        GenderType,
        ModelPath,
    }


    //Player模板
    public enum InitMajorTemplate
    {
        AttId,
        RoleId,
        GenderType,
        ModelPath,
    }


    //本地字符串末班表
    public enum LocalizeTemplate
    {
        AttId,
        Description,
    }

    public enum SkillTemplate
    {
        AttId,
        AnimName,
        SkillResourcePath
    }
    
}