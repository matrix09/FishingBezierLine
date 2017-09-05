namespace Assets.Scripts.ConsoleController.Console
{
    using Assets.Scripts.ConsoleController.History;
    using System;
    using UnityEngine;
    using TypeDefine;

    public class Utils
    { 
        public static void GetImageAndStyleForHistoryItem(LogHistoryItem historyItem, ImageFilesContainer imageFiles, out GUIStyle style, out Texture2D image)
        { 
            style = null;
            image = null;
            switch (historyItem._Type)
            {
                case LogHistoryLogType.Error:
                    image = imageFiles._ErrorIcon;
                    style = GUIStyles.ErrorLabelStyle;
                    return;

                case LogHistoryLogType.Assert:
                    image = imageFiles._AssertIcon;
                    style = GUIStyles.AssertLabelStyle;
                    return;

                case LogHistoryLogType.Warning:
                    image = imageFiles._WarningIcon;
                    style = GUIStyles.WarningLabelStyle;
                    return;

                case LogHistoryLogType.Log:
                    image = imageFiles._InfoIcon;
                    style = GUIStyles.InfoLabelStyle;
                    return;

                case LogHistoryLogType.Exception:
                    image = imageFiles._ExceptionIcon;
                    style = GUIStyles.ExceptionLabelStyle;
                    return;

                case LogHistoryLogType.ConsoleInput:
                    image = imageFiles._ConsoleInputIcon;
                    style = GUIStyles.InfoLabelStyle;
                    return;
            }
            throw new InvalidOperationException("unrecognized log type");
        }
    }
}