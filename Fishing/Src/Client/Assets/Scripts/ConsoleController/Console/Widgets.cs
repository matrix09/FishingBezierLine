using UnityEngine;

namespace Assets.Scripts.ConsoleController.Console
{
    public static class Widgets
    {

        public static string FixedSizeTextField (string text, GUIStyle style, params GUILayoutOption[] options)
        {
            return GUI.TextField(GUILayoutUtility.GetRect (100f, 10000f, style.fontSize + 10, 1000f, style, options), text, style);
        }


        public static bool HeadBarButton (Texture2D icon)
        {
            return HeadBarButton(icon, DebugConsole.Instance._imageFiles._BackgroundGradient, "");
        }

        public static bool HeadBarButton (
                Texture2D icon,
                Texture2D backgroundimage,
                string text
            )
        {
            int padding = 20;
            int iconsize = (int)(1.75 * GUIStyles.HeaderHeight);


            Vector2 vec = new Vector2(
                    iconsize + padding * 2,
                    iconsize
                );

            GUILayoutOption[] optionArray = new GUILayoutOption[] { GUILayout.Width (vec.x)};

            Rect rect = GUILayoutUtility.GetRect(
                    vec.x, vec.y,
                    GUIStyles.HeaderButtonLabelStyle,
                    optionArray
                );
            return Button(rect, icon, backgroundimage, text, iconsize, padding);
        }

        public static bool Button ( 
                        Rect rect,
                        Texture2D icon,
                        Texture2D backgroundImage,
                        string text,
                        int iconsize,
                        int padding

            )
        {

            EventType type = Event.current.type;

            if(type == EventType.MouseDown)
            {
                if (GUI.enabled && rect.Contains (Event.current.mousePosition) )
                {
                    Event.current.Use();
                    return true;
                }
            }
            else if(type == EventType.Repaint)
            {

                if(null != backgroundImage)
                    GUI.DrawTexture(rect, backgroundImage);

                if (null != icon)
                    GUI.DrawTexture(
                                                new Rect(
                                                        rect.x + padding, rect.y + (rect.height - iconsize) * 0.5f,
                                                        iconsize,
                                                        iconsize
                                                    )
                                                ,
                                                icon
                        );
            }

            return false;
        }

    }

}