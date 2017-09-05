
namespace Assets.Scripts.ConsoleController.Console
{
    using TypeDefine;
    using System;
    using UnityEngine;
    internal class HeadBar
    {

        public LogHistoryLogType _LogType = LogHistoryLogType.AllLog;
        public HeadBar ()
        {
        }

        public void OnGUI (bool isfocus)
        {
            GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.Height(GUIStyles.HeaderHeight) };
            GUILayout.BeginHorizontal(GUIStyles.HeaderStyle, optionArray1);
            //close debug console
            if (Widgets.HeadBarButton(DebugConsole.Instance._imageFiles._CloseButton))
            {
                DebugConsole.Instance.BIsVisible = false;
            }

            if(DebugConsole.Instance.BIsMaxximazed)
            {
                if (Widgets.HeadBarButton(DebugConsole.Instance._imageFiles._MinimizeTopIcon))
                {
                    DebugConsole.Instance.Minimize();
                }
            }
            else
            {
                if (Widgets.HeadBarButton(DebugConsole.Instance._imageFiles._MaximizeIcon))
                {
                    DebugConsole.Instance.Maximize();
                }
            }
             
            //show error log
            if (Widgets.HeadBarButton(DebugConsole.Instance._imageFiles._ErrorIcon))
            {
                _LogType = LogHistoryLogType.Error;
            }

            if (Widgets.HeadBarButton(DebugConsole.Instance._imageFiles._ExceptionIcon))
            {
                _LogType = LogHistoryLogType.Exception;
            }

            if (Widgets.HeadBarButton(DebugConsole.Instance._imageFiles._WarningIcon))
            {
                _LogType = LogHistoryLogType.Warning;
            }

            if (Widgets.HeadBarButton(DebugConsole.Instance._imageFiles._RunIcon))
            {
                _LogType = LogHistoryLogType.AllLog;
            }

            if (Widgets.HeadBarButton(DebugConsole.Instance._imageFiles._clean))
            {
                DebugConsole.Instance.CleanLog();
            }


            GUILayout.EndHorizontal();


        }

    }
}

