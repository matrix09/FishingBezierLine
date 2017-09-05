﻿using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(BeizierLine))]

public class BeizierLineEditor : Editor {

    private BeizierLine curve;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private const int lineSteps = 10;
    private const float directionScale = 0.5f;
    void OnEnable ()
    {
        curve = target as BeizierLine;
        curve.Reset();
    }



    void OnSceneGUI()
    {
      //  curve = target as BeizierLine;

        handleTransform = curve.transform;

        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        Vector3 p1 = ShowPoint(1);
        Vector3 p2 = ShowPoint(2);
        Vector3 p3 = ShowPoint(3);
        Handles.color = Color.cyan;

        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p2, p3);

        ShowDirection();

        Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);


        //Handles.color = Color.green;

        //Handles.DrawLine(lineStart, lineStart + curve.GetDirection(0f));

        //Handles.color = Color.white;

        //Vector3 lineStart = curve.GetPoint(0f);

        //for (int i = 1; i<= lineSteps; i++)
        //{
        //    Vector3 lineEnd = curve.GetPoint((float)i/(float)lineSteps);
        //    Handles.color = Color.white;
        //    Handles.DrawLine(lineStart, lineEnd);
        //    //Handles.color = Color.green;
        //    //Handles.DrawLine(lineEnd, lineEnd + curve.GetDirection((float)i/(float)lineSteps));

        //    lineStart = lineEnd;
        //}

    }

    private void ShowDirection ()
    {
        Handles.color = Color.green;
        Vector3 point = curve.GetPoint4(0f);
        Handles.DrawLine(point, point + curve.GetDirection4(0f) * directionScale);
        for(int i = 0; i < lineSteps; i++)
        {
            point = curve.GetPoint4((float)i/(float)lineSteps);
            Handles.DrawLine(point, point + curve.GetDirection4((float)i / (float)lineSteps) * directionScale);
        }

    }

    Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(curve.points[index]);

        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "point");
            EditorUtility.SetDirty(curve);
            curve.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;
    }

	
}

