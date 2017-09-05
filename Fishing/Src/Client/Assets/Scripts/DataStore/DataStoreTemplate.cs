namespace Assets.Scripts.DataStore {

    public enum Common {
        AttId
    }

    //�������ñ�
    public enum ParamTemplate {
        AttId,
        Param
    }

   

    //zxf ��ɫ��������ģ������
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


    //NPCģ��
    public enum InitNpcTemplate
    {
        AttId,
        RoleId,
        GenderType,
        ModelPath,
    }


    //Playerģ��
    public enum InitMajorTemplate
    {
        AttId,
        RoleId,
        GenderType,
        ModelPath,
    }


    //�����ַ���ĩ���
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