using UnityEngine;
using System.Collections;
using System;
using AttTypeDefine;
public class BeizierSpline : MonoBehaviour {

    #region 变量
    [HideInInspector]
    [SerializeField]
    private BezierControlPointMode[] modes;

    [SerializeField]
    private bool loop;
    public bool Loop
    {
        get
        {
            return loop;
        }
        set
        {
            loop = value;
            if(value == true)
            {
                modes[modes.Length - 1] = modes[0];
                SetControlPoint(0, points[0]);
            }
        }
    }

    //[HideInInspector]
    [SerializeField]
    public Vector3[] points;
    public int ControlPointCount
    {
        get
        {
            return points.Length;
        }
    }

    private Vector3 t1 = Vector3.zero;
    private Vector3 t2 = Vector3.zero;
    private Vector3 t3 = Vector3.zero;
    private float aspectRatio = 0f;
    private float ObjRadius = 0f;
    public float WidthRadius
    {
        get
        {
            if (eCoordinateType == TrackCoordinateType.eType_RelativeCamera)
            {
                return (Camera.main.orthographicSize * aspectRatio - ObjRadius);
            }
            else
            {
                return 10f;
            }
               
        }
    }
    
    public float HeightRadius
    {
        get
        {
            if (eCoordinateType == TrackCoordinateType.eType_RelativeCamera)
            {
                return (Camera.main.orthographicSize - ObjRadius);
            }
            else
            {
                return -1f;
            }
            
        }
    }

    public float DeepRadius
    {
        get
        {
            if (eCoordinateType == TrackCoordinateType.eType_RelativeCamera)
            {
                return -1f;
            }
            else
            {
                return 10f;
            }
        } 
    }

    private SplineWalkerBase swb;
    public SplineWalkerBase SWB{
        get
        {
            return swb;
        }
        }
    #endregion

    #region 构造/析构

    void OnEnable () {
        //GlobeHelper.InitializeTransform("Prefabs/Fish/Fishes/EmptyObj", transform, eDisabledType.eType_DirObj);
    }

    void OnDisable(){
        points = null; modes = null; swb = null;

    }
    public static void DisableBeizierSpline (Transform t)
    {
        DisabledContainer.AddToDisabledPool(t.name, t);
    }

    #endregion

    #region 设置曲线属性
    public void Reset()
    {
        if (points == null)
        {
            points = new Vector3[swb.nPointNum];
        }
        else
        {
            if (points.Length != swb.nPointNum)
            {
                Array.Resize(ref points, swb.nPointNum);
            }
        }

        if (null == modes)
        {
            modes = new BezierControlPointMode[swb.nPointNum];
        }
        else
        {
            if (modes.Length != swb.nPointNum)
            {
                Array.Resize(ref modes, swb.nPointNum);
            }
        }

        for (int i = 0; i < swb.nPointNum; i++)
        {
            modes[i] = BezierControlPointMode.Free;
        }
    }

    public void SetSplineData(SplineWalkerBase _swb, float size)
    {
        swb = SplineWalkerBase.CopyTo(_swb);

        Reset();

        //半径
        ObjRadius = size;
        //宽高比
        aspectRatio = (float)Screen.width / (float)Screen.height;
        //曲线obj位置 
       
        if(eCoordinateType == TrackCoordinateType.eType_RelativeCamera)
        {
            gameObject.transform.position = new Vector3(
             Camera.main.transform.position.x,
             Camera.main.transform.position.y,
             Camera.main.transform.position.z + (swb.fCameraDis)
            );
            GenerateRandomPath();
        }
        else if(eCoordinateType == TrackCoordinateType.eType_AbsCoordinate)
        {
            GenerateAbsRandomPath();
        }
    }

    public float GetDt(float progress)
    {
        float f = swb.fSpeed / (progress * progress * t1 + progress * t2 + t3).magnitude;

        return f;
    }

    private void SetTsParams()
    {
        t1 = -3 * points[0] + 9 * points[1] - 9 * points[2] + 3 * points[3];
        t2 = 6 * points[0] - 12 * points[1] + 6 * points[2];
        t3 = -3 * points[0] + 3 * points[1];
    }

    #endregion

    #region 设置在曲线上运动物体的朝向
   
    public int CurveCount
    {
        get
        {
            if(points == null)
            {
                int a = 0;
            }
            return (points.Length - 1) / 3;
        }
    }

    public Vector3 GetVelocity(float t)
    {
        int i = 0;
        if (t > 1f)
        {
            t = 1f;
            i = points.Length - 1;
        }
        else if(t == 1f)
        {
            i = 0;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;

            i = (int)t;

            t -= i;

            i *= 3;
        }

#if UNITY_EDITOR
        Vector3 v = Beizier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t);
        Vector3 v1 = transform.TransformPoint(v);

#endif
        return Beizier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t);
       // return transform.TransformPoint(Beizier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t));
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    #endregion

    #region 生成随机点
    public enum TrackCoordinateType
    {
        eType_RelativeCamera,//相对相机
        eType_AbsCoordinate,//绝对坐标
    }
    /// <summary>
    /// 重新设置随机曲线点
    /// eType_HitWall : 撞到墙之后，随机曲线点
    /// eType_NotHitWall : 没有撞到墙，随机曲线点
    /// </summary>
    public enum eCheckHitWall
    {
        eType_HitWall,
        eType_NotHitWall,
    }

    public TrackCoordinateType eCoordinateType = TrackCoordinateType.eType_AbsCoordinate;

    //重置点坐标， 第一个和第二个点的方向和之前的方向相反。
    public void ResetPathPoint (Transform t, eCheckHitWall type, float progress)
    {
        SetControlPoint(0, t.position);

        //Vector3 v1 = new Vector3(
        //               UnityEngine.Random.Range(0 - WidthRadius, WidthRadius),
        //               0f,
        //               UnityEngine.Random.Range(0 - DeepRadius, DeepRadius)
        //       );

        //float dis = (v1 - t.position).magnitude;

        //Vector3 pos = t.position +  dis * (GetVelocity(1f) * 0.5f * (0 - 1f));

        Vector3 pos = GetControlPoint(2);

        //Vector3 v1 = t.position + (t.position - pos).normalized * (t.position - pos).magnitude;
        Vector3 v1 = Vector3.zero;
        int startNum = 2;

        if (type == eCheckHitWall.eType_NotHitWall)//路线走完了，没有撞墙，那么除了第一个点之外的所有点全部随机
        {
            //v1 = t.position + (((t.position - pos).normalized)*(UnityEngine.Random.Range(2, 5)));
            //SetControlPoint(1, v1);
            startNum = 1;
        }
        else if(type == eCheckHitWall.eType_HitWall)//撞到了墙
        {
            if(progress == 1f)//距离随机，不能太大，否则会导致小熊的路线过于一致
            {
                v1 = t.position + (((t.position - pos).normalized) * (UnityEngine.Random.Range(2, 5)));
            }
            else//没走完，撞墙，必须反向，否则可能会一直撞墙 
            {
                v1 = t.position + ((GetVelocity(progress) * 0.5f * (0 - 1f)) * ((t.position - pos).magnitude));
            }
            SetControlPoint(1, v1);
        }

        for (int i = startNum; i < ControlPointCount - 1; i++)
        {
            Vector3 v = new Vector3(
                        UnityEngine.Random.Range(0 - WidthRadius, WidthRadius),
                        0f,
                        UnityEngine.Random.Range(0 - DeepRadius, DeepRadius)
                );
            SetControlPoint(i, v);
        }

        //最后一个点必须是随机方向，为了让小熊能够尽量在整个地图跑动，避免出现过于集中的情况
        eBirthSide eb = (eBirthSide)UnityEngine.Random.Range((int)eBirthSide.Left, (int)eBirthSide.Bottom);
        Vector3 endPoint = GeAbsTerminalPoint(eb);
        SetControlPoint(ControlPointCount - 1, endPoint);

        SetTsParams();
    }

    //出生点 随机
    void GenerateAbsRandomPath ()
    {
        Vector3 staPoint = GeAbsTerminalPoint(swb.eBirthDir);
        SetControlPoint(0, staPoint);
        Vector3 endPoint = GeAbsTerminalPoint(swb.eDisappearDir);
        SetControlPoint(ControlPointCount - 1, endPoint);
        for (int i = 1; i < ControlPointCount - 1; i++)
        {
            Vector3 v = new Vector3(
                        UnityEngine.Random.Range(0 - WidthRadius, WidthRadius),
                        0f,
                        UnityEngine.Random.Range(0 - DeepRadius, DeepRadius)
                );
            SetControlPoint(i, v);
        }

        SetTsParams();
    }

    Vector3 GeAbsTerminalPoint (eBirthSide side)
    {
        Vector3 v = Vector3.zero;
        if (eCoordinateType == TrackCoordinateType.eType_AbsCoordinate)
        {
            Vector3 midPoint = new Vector3(
              UnityEngine.Random.Range(0 - WidthRadius, WidthRadius),
                        0f,
                        UnityEngine.Random.Range(0 - DeepRadius, DeepRadius)
            );

            switch (side)
            {
                case eBirthSide.Left:
                    {
                        v = new Vector3(
                               UnityEngine.Random.Range(0 - WidthRadius, 0 - WidthRadius/2f),
                              0f,
                              midPoint.z
                          );
                        break;
                    }
                case eBirthSide.Right:
                    {
                        v = new Vector3(
                               UnityEngine.Random.Range(WidthRadius/2f, WidthRadius),
                              0f,
                               midPoint.z
                           );


                        break;
                    }
                case eBirthSide.Top:
                    {
                        v = new Vector3(
                            midPoint.x,
                             0f,
                             UnityEngine.Random.Range(DeepRadius/2f, DeepRadius)
                         );
                        break;
                    }
                case eBirthSide.Bottom:
                    {
                        v = new Vector3(
                              midPoint.x,
                             0f,
                              UnityEngine.Random.Range(0 - DeepRadius, 0 - DeepRadius/2f)
                         );
                        break;
                    }
            }


        }
        return v;
    }

    void GenerateRandomPath ()
    {
        if(eCoordinateType == TrackCoordinateType.eType_RelativeCamera)
        {
            Vector3 staPoint = GetTerminalPoint(swb.eBirthDir);
            //SetControlPoint(0, transform.InverseTransformPoint(staPoint));
            SetControlPoint(0, staPoint);
            Vector3 endPoint = GetTerminalPoint(swb.eDisappearDir);
            //SetControlPoint(ControlPointCount - 1, transform.InverseTransformPoint(endPoint));
            SetControlPoint(ControlPointCount - 1, endPoint);
            for (int i = 1; i < ControlPointCount - 1; i++)
            {
                Vector3 v = new Vector3(
                            UnityEngine.Random.Range(0 - WidthRadius, WidthRadius),
                             UnityEngine.Random.Range(0 - HeightRadius, HeightRadius),
                              Camera.main.transform.position.z + (swb.fCameraDis)
                    );
                //SetControlPoint(i, transform.InverseTransformPoint(v));
                SetControlPoint(i, v);
            }

            SetTsParams();
        }
      

    }

    Vector3 GetTerminalPoint (eBirthSide side)
    {
        Vector3 v = Vector3.zero;
        if (eCoordinateType == TrackCoordinateType.eType_RelativeCamera)
        {
            Vector3 midPoint = new Vector3(
              Camera.main.transform.position.x,
              Camera.main.transform.position.y,
              Camera.main.transform.position.z + (swb.fCameraDis)
            );
           
            switch (side)
            {
                case eBirthSide.Left:
                    {
                        v = new Vector3(
                               midPoint.x - (Camera.main.orthographicSize * aspectRatio + ObjRadius),
                              UnityEngine.Random.Range(0 - HeightRadius, HeightRadius),
                              midPoint.z
                          );
                        break;
                    }
                case eBirthSide.Right:
                    {
                        v = new Vector3(
                                midPoint.x + Camera.main.orthographicSize * aspectRatio + ObjRadius,
                               UnityEngine.Random.Range(0 - HeightRadius, HeightRadius),
                               midPoint.z
                           );


                        break;
                    }
                case eBirthSide.Top:
                    {
                        v = new Vector3(
                             UnityEngine.Random.Range(0 - WidthRadius, WidthRadius),
                             midPoint.y + (Camera.main.orthographicSize + ObjRadius),
                             midPoint.z
                         );
                        break;
                    }
                case eBirthSide.Bottom:
                    {
                        v = new Vector3(
                             UnityEngine.Random.Range(0 - WidthRadius, WidthRadius),
                             midPoint.y - (Camera.main.orthographicSize + ObjRadius),
                             midPoint.z
                         );
                        break;
                    }
            }
           
        }

        return v;

    }

    public Vector3 GetControlPoint (int index)
    {
        return points[index];
    }

    public void SetControlPoint (int index, Vector3 point)
    {
        if (index % 3 == 0)
        {
            Vector3 delta = point - points[index];
            if(Loop)
            {
                if(index == 0)
                {
                    points[points.Length - 1] = point;
                    points[1] += delta;
                    points[points.Length - 2] += delta;
                }
                else if(index == (points.Length - 1))
                {
                    points[0] = point;
                    points[1] += delta;
                    points[points.Length - 2] += delta;

                }
                else
                {
                    if (index > 0)
                    {
                        points[index - 1] += delta;
                    }
                    if (index + 1 < points.Length)
                    {
                        points[index + 1] += delta;
                    }
                }
            }
            else
            {
                if (index > 0)
                {
                    points[index - 1] += delta;
                }
                if (index + 1 < points.Length)
                {
                    points[index + 1] += delta;
                }
            }
           
        }



        points[index] = point;

        //Vector3 test = GetPoint(0);

        EnforceMode(index);

    }

    public Vector3 GetPoint (float t)
    {
        int i = 0;
        if(t >= 1f)
        {
            t = 1f;

            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;

            i = (int)t;

            t -= i;

            i *= 3;

        }

#if UNITY_EDITOR
        Vector3 v = Beizier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t);
        Vector3 v1 = transform.TransformPoint(v);
#endif

        return Beizier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t);
       // return transform.TransformPoint(Beizier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
    }
    #endregion
    
    #region depressed functions
    public void AddCurve ()
    {

        Vector3 point = points[points.Length - 1];

        Array.Resize(ref points, points.Length + 3);

        point = new Vector3(point.x + 1f, point.y, point.z);
        points[points.Length - 3] = point;

        point = new Vector3(point.x + 2f, point.y, point.z);
        points[points.Length - 2] = point;


        point = new Vector3(point.x + 3f, point.y, point.z);
        points[points.Length - 1] = point;

        Array.Resize(ref modes, modes.Length - 1);
        modes[modes.Length - 1] = modes[modes.Length - 2];

        if(Loop)
        {
            points[points.Length - 1] = points[0];
            modes[modes.Length - 1] = modes[0];
            EnforceMode(0);
        }


    }

    public BezierControlPointMode GetControlPointMode (int index)
    {
        BezierControlPointMode mode = modes[(index + 1)/3];
        return modes[(index + 1)/3];
    }

    public void SetControlPointMode (int index, BezierControlPointMode _mode)
    {
        int modelIndex = (index + 1)/3;
        modes[modelIndex] = _mode;
        if(Loop)
        {
            if(modelIndex == 0)
            {
                modes[modes.Length - 1] = _mode;
            }
            else if(modelIndex == modes.Length - 1)
            {
                modes[0] = _mode;
            }
        }
        EnforceMode(index);
    }

    private void EnforceMode (int index)
    {

        int modeIndex = (index + 1)/3;

        BezierControlPointMode mode = modes[index];

        if (mode == BezierControlPointMode.Free || !Loop && (modeIndex == 0 || modeIndex == modes.Length - 1))
            return;

        int middleIndex = modeIndex * 3;

        int fixedIndex, enforcedIndex;
        if(index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            enforcedIndex = middleIndex + 1;
            if(fixedIndex < 0)
            {
                fixedIndex = points.Length - 2;
            }

            if(enforcedIndex >= points.Length)
            {
                enforcedIndex = 1;
            }

        }
        else
        {
            fixedIndex = middleIndex + 1;
            enforcedIndex = middleIndex - 1;

            if (enforcedIndex < 0)
            {
                enforcedIndex = points.Length - 2;
            }

            if (fixedIndex >= points.Length)
            {
                fixedIndex = 1;
            }

        }

        Vector3 middle = points[middleIndex];
        Vector3 enforcedTangent = middle - points[fixedIndex];

        if(mode == BezierControlPointMode.Aligned)
        {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
        }

        points[enforcedIndex] = middle + enforcedTangent;
    }
    #endregion
}
