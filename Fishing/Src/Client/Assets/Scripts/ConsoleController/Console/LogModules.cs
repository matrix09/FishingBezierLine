using UnityEngine;
using System.Collections;
using Assets.Scripts.ConsoleController.Console;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.ConsoleController.History;
using TypeDefine;

namespace Assets.Scripts.ConsoleController.Command
{
    internal class LogModules
    {

        private Vector2 _scrollPos;
        private bool bIsVisible = false;
        public bool BIsVisible
        {
            get
            {
                return bIsVisible;
            }
            set
            {
                if(bIsVisible && !value)
                {
                    CommitLogModules();
                }
                else if(!bIsVisible && value)
                {
                    string _log = string.Format("Open Log Modules:)");
                    LogHistoryItem item = new LogHistoryItem(LogHistoryLogType.ConsoleInput, _log, Time.time);
                    DebugConsole.Instance.AddItem(item);
                }

                bIsVisible = value;
            }

        }

        private LogHistoryView _LogHistoryView;

        private LogHistory _LogHistory;

        public LogModules (LogHistoryView _logHistoryView, LogHistory _loghistory)
        {
            
            _LogHistoryView = _logHistoryView;

            _LogHistory = _loghistory;

          

        }

        private string GetKeys (KeyValuePair<string, bool> i)
        {
            return i.Key;
        }

        private bool GetValues (KeyValuePair<string, bool> i)
        {
            return i.Value;
        } 

        public void OnGUI (Rect rect)
        {
            if (bIsVisible == false)
                return;


            GUILayout.BeginArea(rect);

            _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUIStyles.LogModulesBackGroundStyle);


            GUILayout.BeginHorizontal();

            string[] keys = Enumerable.Select(from i in DebugHelper._Modules select i,
                key => GetKeys(key)).ToArray();

            bool[] values = Enumerable.Select(from i in DebugHelper._Modules select i,
                    value => GetValues(value)).ToArray();

            int count = Mathf.Min(keys.Length, values.Length);

           
            for (int i = 0; i < count; i++)
            {
                bool bChanged = false;
                string text = keys[i];

                GUILayoutOption[] options = new GUILayoutOption[] {
                                                                                                            GUILayout.ExpandWidth(false),
                                                                                                            GUILayout.Height(GUIStyles.SuggestionButtonHeight)
                                                                                                            };

                bChanged = GUILayout.Toggle(DebugHelper._Modules[text], text, options);
                if(bChanged != DebugHelper._Modules[text])
                {
                    DebugHelper.ToggleModule(text);
                   
                }

            }

            GUILayout.EndHorizontal();


            GUILayout.EndScrollView();

 
       


            GUILayout.EndArea();

        }

        private void CommitLogModules ()
        {

            string[] keys = Enumerable.Select(from i in DebugHelper._Modules select i,
             key => GetKeys(key)).ToArray();

            bool[] values = Enumerable.Select(from i in DebugHelper._Modules select i,
                    value => GetValues(value)).ToArray();

            int count = Mathf.Min(keys.Length, values.Length);

            int index = 0;
            for (int i = 0; i < count; i++)
            {
                if(true == (DebugHelper._Modules[keys[i]] ? true : false))
                {
                    string _log = string.Format("Debug log modules toggled:{0}:{1}", keys[i], DebugHelper.IsModules(keys[i]));
                    LogHistoryItem item = new LogHistoryItem(LogHistoryLogType.ConsoleInput, _log, Time.time);
                    DebugConsole.Instance.AddItem(item);
                    index++;
                }  
            }

            if (0 == index)
            {
                string _log = string.Format("No Modules Selected, No one is gonna shown:)");
                LogHistoryItem item = new LogHistoryItem(LogHistoryLogType.ConsoleInput, _log, Time.time);
                DebugConsole.Instance.AddItem(item);
            }

        }

        //[deprecated]
        //private void HandlePresEvent(string key)
        //{
        //    DebugHelper.ToggleModule(key);

        //    string _log = string.Format("Debug log modules toggled:{0}:{1}", key, DebugHelper.IsModules(key));
        //    LogHistoryItem item = new LogHistoryItem(LogHistoryLogType.ConsoleInput, _log, Time.time);
        //    DebugConsole.Instance.AddItem(item);
        //    Event.current.Use();
        //}






    }

}
