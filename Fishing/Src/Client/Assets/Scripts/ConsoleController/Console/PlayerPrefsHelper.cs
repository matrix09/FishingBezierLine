//
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;

namespace Assets.Scripts.ConsoleController.Console
{
    public sealed  class PlayerPrefsHelper
    {

        private class ListHelper<T>
        {
#pragma warning disable 414
            public List<T> list;
#pragma warning restore 414
        }

        public static void SetList<T>(string key, List<T> value)
        {
            var val = new ListHelper<T> { list = value };
            var json = JsonUtility.ToJson(val);
            PlayerPrefs.SetString(key, json);
        }

        public static List<T> GetList<T>(string key)
        {
            var json = PlayerPrefs.GetString(key);
            var obj = JsonUtility.FromJson<ListHelper<T>>(json);
            return obj == null ? new List<T>() : obj.list;
        }

    }

}