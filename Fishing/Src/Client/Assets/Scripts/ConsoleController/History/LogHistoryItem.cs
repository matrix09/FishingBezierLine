namespace Assets.Scripts.ConsoleController.History
{
    using System;
    using System.Runtime.InteropServices;
    using TypeDefine;
    public class LogHistoryItem
    {
        private static int _Counter;
        public string _FirstLineOfLogMessage;
        public int _Id = _Counter++;
        public string _LogMessage;
        public string _StackTrace;
        public float _Time;
        public LogHistoryLogType _Type;

        public LogHistoryItem(LogHistoryLogType type, string message, float time, string stackTrace = "")
        {
            this._Type = type;
            this._LogMessage = message;
            this._StackTrace = stackTrace;
            char[] separator = new char[] { '\n' };
            this._FirstLineOfLogMessage = this._LogMessage.Split(separator, 2)[0];
            this._Time = time;
        }
    }
}
