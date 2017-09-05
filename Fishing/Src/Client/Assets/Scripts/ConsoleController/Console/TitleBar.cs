namespace Assets.Scripts.ConsoleController.Console
{
    using UnityEngine;
    using System.Collections;

    internal class TitleBar
    {
        public TitleBar()
        {

        }

        public void OnGUI()
        {

            string str = string.Format("Debug Console v{0}\tfps: {1:00.0}", 1.0, DebugConsole.Instance.fps.current);


            //确定text editor绘制风格
            GUILayoutOption[] options = new GUILayoutOption[] {
                GUILayout.Height(GUIStyles.TitleHeight),
                GUILayout.ExpandWidth(true) };

            GUI.TextField(GUILayoutUtility.GetRect(100f, 10000f, 
                GUIStyles.TitleBarFieldStyle.fontSize + 10, 1000f,
                GUIStyles.TitleBarFieldStyle, options), 
                str, 
                GUIStyles.TitleBarFieldStyle);



        }
    }
}