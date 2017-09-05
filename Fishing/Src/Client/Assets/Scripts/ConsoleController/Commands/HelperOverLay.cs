/*
功能设定 ：
1 :弹出命令行预设界面， console以小窗口显示
即 ： HelperOverLay.bVisible == true  -> DebugConsole.bIsMaximize = false
*/

using System;
using System.Collections.Generic;
using UnityEngine;
//using System.Collections;
//using Assets.Scripts.ConsoleController.Console;
namespace Assets.Scripts.ConsoleController.Command
{
    internal class HelperOverLay
    {
        private Vector2 _scrollPos = Vector2.zero;

        private readonly  List<CommandHandler> _sortedCommandList = new List<CommandHandler>();

        public HelperOverLay( )
        {
            
            CommandHandlers.CommandHandlerAdded += new Action<CommandHandler>(OnCommandHandlerAdded);
            CommandHandlers.CommandHandlerRemoved += new Action<CommandHandler>(OnCommandHandlerRemoved);
            _sortedCommandList.AddRange(CommandHandlers.Handlers);
        }

        private void OnCommandHandlerAdded (CommandHandler _handle)
        {
            _sortedCommandList.Add(_handle);
        }

        private void OnCommandHandlerRemoved (CommandHandler _handle)
        {
            _sortedCommandList.Remove(_handle);
        }

        private bool bvisible;
        public bool bIsVisible {
            get
            {
                return bvisible;
            }
            set
            {
                if(value == true)
                {
                    DebugConsole.Instance.Minimize();
                }
                bvisible = value;
                
            }
        }

        public void OnGUI(Rect rect)
        {
            //绘制rect区域
            GUILayout.BeginArea(rect);
            //横向显示预设命令
            _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUIStyles.HelpWindowBackGroundStyle);
            //检测按键事件，是否选择预设命令
            GUILayout.BeginHorizontal();
            foreach (CommandHandler handler in _sortedCommandList)
            {

                string text = handler.CommandName;

                GUILayoutOption[] options = new GUILayoutOption[] {
                                                                                                        GUILayout.ExpandWidth(false),
                                                                                                        GUILayout.Height(GUIStyles.SuggestionButtonHeight)
                                                                                                        };

                GUILayout.Label(text, options);

                if((Event.current.type == EventType.mouseDown) && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition)) {

                    HandlePresEvent(handler);

                }

            }

            GUILayout.Space(20);

            //遍歷所有log modules


            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        public void HandlePresEvent (CommandHandler handler)
        {
            //关闭预设UI
            bIsVisible = false;

            //commandname 复制到command line上面
            DebugConsole.Instance.SetCommandLineText(handler);

            Event.current.Use();

        }


    }
}

