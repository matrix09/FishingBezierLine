
namespace Assets.Scripts.ConsoleController.History
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using TypeDefine;
    using Assets.Scripts.ConsoleController.Console;
    internal class LogHistory : IDisposable
    {

        private List<LogHistoryItem> _logHistoryItems = new List<LogHistoryItem>();
     

        public int AllLogCount
        {
            get
            {
                return _logHistoryItems.Count;
            }
        }

        private List<LogHistoryItem> _CurrentHistory = null;
        private LogHistoryLogType _LogType = LogHistoryLogType.None;

        private HeadBar _HeadBar;

        private List<LogHistoryItem> _FilterHistoryItems = new List<LogHistoryItem>();
        public List<LogHistoryItem> _FilterHistoryView
        {
            get
            {
                return _FilterHistoryItems;
            }
        }
        public int LogCount
        {
            get
            {
                if(null != _CurrentHistory)
                {
                    return _CurrentHistory.Count;   
                }
                else
                {
                    return 0;
                }
                
            }
        }

        public LogHistoryItem this[int index]
        {
            get
            {

                if(index < 0 || index >=_CurrentHistory.Count)
                {
                    return null;
                }
                else
                {
                    return _CurrentHistory[index];
                }

               
            }
        }

        public int GetIndexByItem(LogHistoryItem item)
        {
            return _CurrentHistory.IndexOf(item);
        }

        public LogHistory(HeadBar _bar)
        {

            _HeadBar = _bar;

            LogHandler.RegisterLogCallback(new Application.LogCallback(HandleLoggingEvent));

        }


        public void AddCommandLine (LogHistoryItem item)
        {
            _logHistoryItems.Add(item);
        }

        public List<LogHistoryItem> GetHistoryList(out bool bChanged)
        {

            bChanged = false;

            if (_HeadBar._LogType != LogHistoryLogType.AllLog)
            {
                if (_LogType != _HeadBar._LogType)
                {
                    bChanged = true;
                    _FilterHistoryItems.Clear();
                    for (int i = 0; i < _logHistoryItems.Count; i++)
                    {
                        LogHistoryItem item = _logHistoryItems[i];
                        if (_HeadBar._LogType == item._Type)
                        {
                            _FilterHistoryItems.Add(item);
                        }
                    }

                    _LogType = _HeadBar._LogType;

                    _CurrentHistory = _FilterHistoryItems;
                }
            }
            else
            {
                if (_LogType != _HeadBar._LogType)
                {
                    bChanged = true;
                    _LogType = _HeadBar._LogType;
                }
                    _CurrentHistory = _logHistoryItems;
            }

            return _CurrentHistory;

        }



        public bool CheckFilter(LogHistoryLogType type)
        {
            if (_HeadBar._LogType == LogHistoryLogType.AllLog)
                return true;

            if (_HeadBar._LogType == type)
                return true;

            return false;

        }

        private void HandleLoggingEvent(string logString, string stackTrace, LogType type)
        {
            InternalHandleLoggingEvent(logString, stackTrace, this.ConvertFromUnityLogType(type));
        }

        private void InternalHandleLoggingEvent(string logString, string stackTrace, LogHistoryLogType type)
        {
            LogHistoryItem item = new LogHistoryItem(type, logString, Time.time, (stackTrace));
            _logHistoryItems.Add(item);
        }

        public LogHistoryLogType ConvertFromUnityLogType(LogType logType)
        {
            switch (logType)
            {
                case LogType.Error:
                    return LogHistoryLogType.Error;

                case LogType.Assert:
                    return LogHistoryLogType.Assert;

                case LogType.Warning:
                    return LogHistoryLogType.Warning;

                case LogType.Log:
                    return LogHistoryLogType.Log;

                case LogType.Exception:
                    return LogHistoryLogType.Exception;
            }
            throw new InvalidOperationException("Unrecognized log type " + logType);
        }

        private void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.LogException((Exception)e.ExceptionObject);
        }

        public void Dispose()
        {

            LogHandler.UnRegisterLogCallback(new Application.LogCallback(HandleLoggingEvent));
        }

        public void ClearAllLog()
        {
            _logHistoryItems.Clear();
            _FilterHistoryItems.Clear();
            _CurrentHistory = null;
        }

    }
}

