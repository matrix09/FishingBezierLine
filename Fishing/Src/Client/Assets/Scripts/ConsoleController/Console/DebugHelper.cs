using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Assets.Scripts.ConsoleController.Console
{
 
    public static class DebugHelper
    {
        private static Dictionary<string, bool> _modules = new Dictionary<string, bool>();

        public static Dictionary<string, bool> _Modules
        {
            get
            {
                return _modules;
            }
        }


       public static void ToggleModule (string key)
        {
            if(_modules.ContainsKey(key))
            {
                _modules[key] = !_modules[key];

                SaveOptions();
            }



        }

        public static bool IsModules (string _strModules)
        {
            if(_modules.Count == 0)
            {
                LoadOptions();
            }

            if(!_modules.ContainsKey(_strModules))
            {
                _modules.Add(_strModules, false);
                SaveOptions();
            }


            return _modules[_strModules];

        }


        private static void LoadOptions()
        {

            var keys = PlayerPrefsHelper.GetList<string>(typeof(DebugHelper).FullName + ":keys").ToList();
            var values = PlayerPrefsHelper.GetList<bool>(typeof(DebugHelper).FullName + ":values").ToList();

            _modules.Clear();
            for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
            {
                _modules.Add(keys[i], values[i]);
            }
        }

        private static void SaveOptions ()
        {

            PlayerPrefsHelper.SetList(typeof(DebugHelper).FullName + ":keys", _modules.Keys.ToList());

            PlayerPrefsHelper.SetList(typeof(DebugHelper).FullName + ":values", _modules.Values.ToList());

        }

        public static void Log(string module, string msg)
        {
            if (!IsModules(module))
            {
                return;
            }

            Debug.Log(String.Format("[{2}:{0}] - {1}", module, msg, DateTime.Now.ToString("HH:mm:ss.ffff")));
        }

        public static void LogFormat(string module, string format, params object[] args)
        {

            Log(module, string.Format(format, args));
        }


        public static void Assert (string module, bool condition)
        {
            if (!IsModules(module))
            {
                return;
            }

            Debug.Assert(condition);
        }

        public static void AssertFormat (string module, bool condition, string msg, params object[] args)
        {

            if (!IsModules(module))
            {
                return;
            }

            Debug.AssertFormat(condition, msg, args);
        }


        public static void Warn(string module, string msg)
        {
            if (!IsModules(module))
            {
                return;
            }

            Debug.LogWarning(String.Format("[{2}:{0}] - {1}", module, msg, DateTime.Now.ToString("HH:mm:ss.ffff")));
        }

        public static void WarnFormat(string module, string format, params object[] args)
        {

            Warn(module, string.Format(format, args));
        }

        public static void Error(string module, string msg)
        {
            if (!IsModules(module))
            {
                return;
            }

            Debug.LogError(String.Format("[{2}:{0}] - {1}", module, msg, DateTime.Now.ToString("HH:mm:ss.ffff")));
        }

        public static void ErrorFormat(string module, string format, params object[] args)
        {

            Error(module, string.Format(format, args));
        }

        public static void Log(string msg)
        {

            Log("Normal", msg);
        }

        public static void Log(this object ob, string module, string msg)
        {
            LogFormat(module, "[{0}:{1}] -- {2}", ob.GetHashCode(), ob.GetType().Name, msg);
        }

        public static void LogFormat(this object ob, string module, string format, params object[] args)
        {
            LogFormat(module, "[{0}:{1}] -- {2}", ob.GetHashCode(), ob.GetType().Name, string.Format(format, args));
        }

        public static void Warn(this object ob, string module, string msg)
        {
            WarnFormat(module, "[{0}:{1}] -- {2}", ob.GetHashCode(), ob.GetType().Name, msg);
        }

        public static void WarnFormat(this object ob, string module, string format, params object[] args)
        {
            WarnFormat(module, "[{0}:{1}] -- {2}", ob.GetHashCode(), ob.GetType().Name, string.Format(format, args));
        }

        public static void Error(this object ob, string module, string msg)
        {
            ErrorFormat(module, "[{0}:{1}] -- {2}", ob.GetHashCode(), ob.GetType().Name, msg);
        }

        public static void ErrorFormat(this object ob, string module, string format, params object[] args)
        {
            ErrorFormat(module, "[{0}:{1}] -- {2}", ob.GetHashCode(), ob.GetType().Name, string.Format(format, args));
        }

        public static void Assert(this object ob,string module, bool condition)
        {
            Assert(module, condition);
        }

        public static void AssertFormat(this object ob, string module, bool condition, string format, params object[] args)
        {
            AssertFormat(module, condition, "[{0}:{1}] -- {2}", ob.GetHashCode(), ob.GetType().Name, string.Format(format, args));
        }

    }
}

    
