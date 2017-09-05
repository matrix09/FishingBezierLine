using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers.Attributes;
using AttTypeDefine;
using Assets.Scripts.DataStore;
using Assets.Scripts.ConsoleController.Console;
public class SplineWalkerBase :  BaseClass{

    #region 变量
    public int AttId;
    //-----------------------------模板属性-------------------------//
    //speed scope
    private float MinSpeed;
    private float MaxSpeed;
 
    //camera distance scope
    private float MinCameraDistance;
    private float MaxCameraDistance;

    //point scope
    private int MinPointNum;
    private int MaxPointNum;

    //first and last point dir scope
    private eBirthSide MinDirValue;
    private eBirthSide MaxDirValue;
    private bool isValid = false;
    //---------------------------------------------------------------//

    public float fSpeed;
    public float fCameraDis;
    public int nPointNum;
    public bool bIsLoop;
    public string fishRoute;
    public eBirthSide eBirthDir;
    public eBirthSide eDisappearDir;
    #endregion

    #region 构造函数
    public SplineWalkerBase(){}
    //获取模板数据
    public SplineWalkerBase (int id)
    {
        var record = DataRecordManager.GetDataRecord<SplineWalkerTemplate>(id, out isValid);
        if (null != record && isValid)
        {
            this.LogFormat("DataStore", "");
            //曲线id
            AttId = record.GetInt(SplineWalkerTemplate.AttId);

            //曲线最小速度
            int speed = record.GetInt(SplineWalkerTemplate.MinSpeed);
            MinSpeed = (float)speed;
            MinSpeed /= 100f;
           //曲线最大速度
            speed = record.GetInt(SplineWalkerTemplate.MaxSpeed);
            MaxSpeed = (float)speed;
            MaxSpeed /= 100f;

            //fSpeed = GetSpeed();

            //相机最近/最远距离
            MinCameraDistance = record.GetInt(SplineWalkerTemplate.MinCameraDistance);
            MaxCameraDistance = record.GetInt(SplineWalkerTemplate.MaxCameraDistance);

            //fCameraDis = GetCameraDis();

            MinPointNum = record.GetInt(SplineWalkerTemplate.MinPointNum);
            MaxPointNum = record.GetInt(SplineWalkerTemplate.MaxPointNum);

            //nPointNum = GetPointNum();

            //曲线方向最大最小取值范围
            int minDir = record.GetInt(SplineWalkerTemplate.MinDirValue);
            MinDirValue = (eBirthSide)minDir;
            int MaxDir = record.GetInt(SplineWalkerTemplate.MaxDirValue);
            MaxDirValue = (eBirthSide)MaxDir;

            //eBirthDir = GetDirValue();
            //eDisappearDir = GetDirValue();

            //曲线是否收尾相接
            int loop = record.GetInt(SplineWalkerTemplate.BIsCycle);
            bIsLoop = (loop == 0) ? false : true;

            //实例路径
            fishRoute = record.GetString(SplineWalkerTemplate.FishRoute);

            this.LogFormat("SplineWalkerBase", 
                "fSpeed = {0}, fCameraDis = {1}, nPointNum = {2}, BirthDir = {3},DisappearDir = {4}",
                fSpeed, fCameraDis, nPointNum, eBirthDir, eDisappearDir);
        }
    }
    #endregion

    #region 获取随机属性
    float GetSpeed ()
    {
        return Random.Range(MinSpeed, MaxSpeed);
    }

     float GetCameraDis ()
    {
        return Random.Range(MinCameraDistance, MaxCameraDistance);
    }

     eBirthSide GetDirValue ()
    {
        int n = Random.Range((int)eBirthSide.Start, (int)eBirthSide.End + 1);
        return (eBirthSide)n;
    }

    //TODO
     int GetPointNum ()
    {
        int n = Random.Range(MinPointNum, MaxPointNum + 1);
        //return (n * 3 + 1);
        return 4;
    }
    #endregion

    #region 将模板数据复制给新的SplineWalkerBase实例
    public static SplineWalkerBase CopyTo (SplineWalkerBase swb)
    {
        SplineWalkerBase _swb = new SplineWalkerBase();
        _swb.fSpeed = swb.GetSpeed();
        _swb.fCameraDis = swb.GetCameraDis();
        _swb.nPointNum = swb.GetPointNum();
        _swb.eBirthDir = swb.GetDirValue();
        _swb.eDisappearDir = swb.GetDirValue();
        return _swb;
    }
    #endregion

}


