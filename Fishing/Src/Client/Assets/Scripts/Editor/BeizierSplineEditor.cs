using UnityEngine;
using System.Collections;
using UnityEditor;
using AttTypeDefine;
[CustomEditor(typeof(BeizierSpline))]
public class BeizierSplineEditor : Editor
{
    private const int lineSteps = 10;
    private const float directionScale = 0.5f;
    private BeizierSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private static Color[] modeColors = {
            Color.white,
            Color.yellow,
            Color.cyan
    };

    void Awake ()
    {
        spline = target as BeizierSpline; 
    }

    Vector3 p0, p1, p2, p3;
    void OnSceneGUI ()
    {
        handleTransform = spline.transform;

        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

   
        p0 = ShowPoint(0);

        for (int i = 1; i < spline.ControlPointCount; i+= 3)
        {
            p1 = ShowPoint(i);
            p2 = ShowPoint(i + 1);
            p3 = ShowPoint(i + 2);

            Handles.color = Color.blue;

            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
        }

        //ShowDirections();
    }

    //只处理起点坐标 
    private void ShowDirections ()
    {

        Handles.color = Color.cyan;

        Vector3 v = spline.GetPoint(0f);

        Handles.DrawLine(v, v + spline.GetDirection(0f) * directionScale);

        for(int i = 1; i < lineSteps; i++)
        {
            v = spline.GetPoint((float)i/(float)lineSteps);
            Handles.DrawLine(v, v + spline.GetDirection((float)i / (float)lineSteps) * directionScale);
            Debug.Log(spline.GetDirection((float)i / (float)lineSteps));
        }
    }

    private const float handleSize = 0.04f;
    private const float pickSize = 0.06f;
    private int selectedIndex = -1;
    private Vector3 ShowPoint (int index)
    {
        Vector3 v = Vector3.zero;

        v = (spline.GetControlPoint(index));
        Handles.color = modeColors[(int)spline.GetControlPointMode(index)];
        float size = HandleUtility.GetHandleSize(v);
        if(index == 0)
        {
            size *= 2f;
        }
        if(Handles.Button(v, handleRotation, handleSize * size, pickSize * size, Handles.DotCap))
        {
            selectedIndex = index;
            Repaint();
        }

        if(selectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();

            v = Handles.DoPositionHandle(v, handleRotation);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "bezier spline");
                EditorUtility.SetDirty(spline);
                spline.SetControlPoint(index, v);
            }
        }

        return v;
    }

    #region 设置inspector风格
    private GUIStyle LabelStyle;
    private GUIStyle TipsStyle = null;
    private void SetStyles ()
    {
        if(LabelStyle == null)
        {
            LabelStyle = new GUIStyle(GUI.skin.label);
            LabelStyle.normal.textColor = Color.green;
            LabelStyle.focused.textColor = Color.green;
            LabelStyle.active.textColor = Color.green;
            LabelStyle.fontSize = 25;
            LabelStyle.alignment = TextAnchor.UpperLeft;
        }

        if(TipsStyle == null)
        {
            TipsStyle = new GUIStyle(GUI.skin.label);
            TipsStyle.fontSize = 16;
            TipsStyle.normal.textColor = Color.cyan;
            TipsStyle.wordWrap = true;
            TipsStyle.alignment = TextAnchor.UpperLeft;
        }
    }

    
    #endregion

    //public override void OnInspectorGUI()
    //{
        //SetStyles();
        //EditorGUILayout.Space(); EditorGUILayout.Space();
        //GUILayoutOption[] option = new GUILayoutOption[] { GUILayout.Height(40) };

        //EditorGUILayout.LabelField("曲线基本属性", LabelStyle, option);

        //option = new GUILayoutOption[] { GUILayout.Height(20) };
        //1 绘制f speed
        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("曲线速度", option);
        //float fSpeed = EditorGUILayout.Slider(spline.SWB.fSpeed, 0f, 1f, option);
        //if(fSpeed != (spline.fSpeed))
        //{
        //    spline.fSpeed = (fSpeed);
        //    EditorUtility.SetDirty(spline);
        //}
        //EditorGUILayout.EndVertical();
        //EditorGUILayout.BeginVertical("HelpBox");
        //EditorGUILayout.LabelField("速度取值范围0~1。数字越小，速度越小，反之，数字越大，速度越大。", TipsStyle);
        //EditorGUILayout.EndVertical();

        //2 绘制是否loop
        //EditorGUI.BeginChangeCheck();
        //bool loop = EditorGUILayout.Toggle("曲线是否首尾相接", spline.Loop);
        //if (EditorGUI.EndChangeCheck())
        //{
        //    Undo.RecordObject(spline, "Toggle Loop");
        //    EditorUtility.SetDirty(spline);
        //    spline.Loop = loop;
        //}

        ////3 绘制曲线距离相机距离
        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("曲线和相机的距离", option);
        //EditorGUI.BeginChangeCheck();
        //float fCameraDistance = EditorGUILayout.FloatField(spline.fCameraDistance, option);
        //if (EditorGUI.EndChangeCheck())
        //{
        //    Undo.RecordObject(spline, "Toggle Camera Distance");
        //    EditorUtility.SetDirty(spline);
        //    spline.fCameraDistance = fCameraDistance;
        //}
        //if(fCameraDistance != spline.fCameraDistance)
        //{
        //    spline.fCameraDistance = fCameraDistance;
        //    EditorUtility.SetDirty(spline);
        //}
        //EditorGUILayout.EndVertical();
        //4 绘制出生方向
        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("曲线轨迹方向", option);
        //AttTypeDefine.eBirthSide eBirthFromSide =  (AttTypeDefine.eBirthSide)EditorGUILayout.EnumPopup(spline.eBirthFromSide, option);
        //if(eBirthFromSide != spline.eBirthFromSide)
        //{
        //    spline.eBirthFromSide = eBirthFromSide;
        //    EditorUtility.SetDirty(spline);
        //}
        //EditorGUILayout.EndVertical();
        //5 绘制消失方向

        //EditorGUILayout.Space(); EditorGUILayout.Space();

        ////绘制选中的点坐标和mode
        //if (selectedIndex >= 0 && selectedIndex < spline.ControlPointCount)
        //{
        //    DrawSelectedPointInspector();
        //}

        //if(GUILayout.Button("Add Curve"))
        //{
        //    Undo.RecordObject(spline, "Add Curve");

        //    spline.AddCurve();

        //    EditorUtility.SetDirty(spline);
        //}
    //}

    private void DrawSelectedPointInspector ()
    {
        GUILayoutOption[] option = new GUILayoutOption[] { GUILayout.Height(40) };

        EditorGUILayout.LabelField("绘制选中的点坐标和mode", LabelStyle, option);

        option = new GUILayoutOption[] { GUILayout.Height(20) };

        EditorGUI.BeginChangeCheck();

        Vector3 point = EditorGUILayout.Vector3Field("选中的点坐标", spline.GetControlPoint(selectedIndex), option);

        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "move point");
            EditorUtility.SetDirty(spline);
            spline.SetControlPoint(selectedIndex, point);
        }

        EditorGUI.BeginChangeCheck();

        BezierControlPointMode mode = (BezierControlPointMode)EditorGUILayout.EnumPopup("点坐标模式", 
            spline.GetControlPointMode(selectedIndex), option);

        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "change spline mode");
            EditorUtility.SetDirty(spline);
            spline.SetControlPointMode(selectedIndex, mode);
        }
    }

}
