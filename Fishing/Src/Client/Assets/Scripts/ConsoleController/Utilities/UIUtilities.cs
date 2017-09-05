
namespace Assets.Scripts.Utilities
{
    using UnityEngine;
    public static class UIUtilities
    {
        private static GUIContent _guiContent = new GUIContent();

        public static Texture2D CreateTexture(Color col)
        {
            return CreateTexture(1, 1, col);
        }

        public static Texture2D CreateTexture(int width, int height, Color col)
        {
            Color[] colors = new Color[width * height];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = col;
            }
            Texture2D textured1 = new Texture2D(width, height)
            {
                wrapMode = TextureWrapMode.Repeat
            };
            textured1.SetPixels(colors);
            textured1.Apply();
            textured1.hideFlags = HideFlags.HideAndDontSave;
            return textured1;
        }

        public static GUIContent TempGUIContent(string text)
        {
            _guiContent.text = text;
            _guiContent.image = null;
            return _guiContent;
        }

        public static GUIContent TempGUIContent(Texture2D image)
        {
            _guiContent.text = "";
            _guiContent.image = image;
            return _guiContent;
        }

        public static GUIContent TempGUIContent(string text, Texture2D image)
        {
            _guiContent.text = text;
            _guiContent.image = image;
            return _guiContent;
        }

    }
}


