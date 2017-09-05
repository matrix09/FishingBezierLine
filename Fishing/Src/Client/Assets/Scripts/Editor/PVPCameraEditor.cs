using UnityEngine;
using System.Collections;
using UnityEditor;
using Assets.Scripts.Managers.Camera;
[CustomEditor(typeof(PVPCamera))]
public class PVPCameraEditor : Editor
{

    PVPCamera camera;


    #region 设置inspector风格
    private GUIStyle LabelStyle;
    private GUIStyle TipsStyle = null;
    private void SetStyles()
    {
        if (LabelStyle == null)
        {
            LabelStyle = new GUIStyle(GUI.skin.label);
            LabelStyle.normal.textColor = Color.green;
            LabelStyle.focused.textColor = Color.green;
            LabelStyle.active.textColor = Color.green;
            LabelStyle.fontSize = 25;
            LabelStyle.alignment = TextAnchor.UpperLeft;
        }

        if (TipsStyle == null)
        {
            TipsStyle = new GUIStyle(GUI.skin.label);
            TipsStyle.fontSize = 16;
            TipsStyle.normal.textColor = Color.cyan;
            TipsStyle.wordWrap = true;
            TipsStyle.alignment = TextAnchor.UpperLeft;
        }
    }


    #endregion

    public override void OnInspectorGUI()
    {
        camera = target as PVPCamera;

        SetStyles();
        EditorGUILayout.Space(); EditorGUILayout.Space();
        GUILayoutOption[] option = new GUILayoutOption[] { GUILayout.Height(40) };
        EditorGUILayout.LabelField("跟随相机基础参数", LabelStyle, option);

        option = new GUILayoutOption[] { GUILayout.Height(20) };
        //1 绘制相对主角的坐标偏移
        EditorGUILayout.BeginHorizontal();
        Vector3 offset = EditorGUILayout.Vector3Field("相机偏移坐标", camera.vOffSet, option);
        if (offset != camera.vOffSet)
        {
            camera.vOffSet = offset;
        }
        EditorGUILayout.EndVertical();

        //2 绘制相机旋转值
        EditorGUILayout.BeginHorizontal();
        Vector3 vRot = EditorGUILayout.Vector3Field("相机旋转坐标", camera.vRot, option);
        if (vRot != camera.vRot)
        {
            camera.vRot = vRot;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("相机类型", option);
        AttTypeDefine.eCamType CamType = (AttTypeDefine.eCamType)EditorGUILayout.EnumPopup(camera.CamType, option);
        if (CamType != camera.CamType)
        {
            camera.CamType = CamType;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();
        Vector3 vSmooth = EditorGUILayout.Vector3Field("相机跟随平滑值 ", camera.vSmooth, option);
        if (vSmooth != camera.vSmooth)
        {
            camera.vSmooth = vSmooth;
        }
        EditorGUILayout.EndVertical();



    }

}
