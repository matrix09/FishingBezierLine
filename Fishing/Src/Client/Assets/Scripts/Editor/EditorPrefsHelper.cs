using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

namespace Assets.Scripts.Helpers {
    /// <summary>
    /// 
    /// <para>
    /// Stores and accesses Unity editor preferences.
    /// </para>
    /// 
    /// </summary>
    public sealed class EditorPrefsHelper {
        /// <summary>
        /// 
        /// <para>
        /// Sets the value of the preference identified by key as an integer.
        /// </para>
        /// 
        /// </summary>
        /// <param name="key"/><param name="value"/>
        public static void SetInt( string key, int value ) {
            EditorPrefs.SetInt(key, value);
        }

        /// <summary>
        /// 
        /// <para>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </para>
        /// 
        /// </summary>
        /// <param name="key"/><param name="defaultValue"/>
        public static int GetInt( string key, [DefaultValue("0")] int defaultValue ) {
            return EditorPrefs.GetInt(key, defaultValue);
        }

        /// <summary>
        /// 
        /// <para>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </para>
        /// 
        /// </summary>
        /// <param name="key"/>
        public static int GetInt( string key ) {
            return EditorPrefs.GetInt(key);
        }

        /// <summary>
        /// 
        /// <para>
        /// Sets the value of the preference identified by key.
        /// </para>
        /// 
        /// </summary>
        /// <param name="key"/><param name="value"/>
        public static void SetFloat( string key, float value ) {
            EditorPrefs.SetFloat(key, value);
        }

        /// <summary>
        /// 
        /// <para>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </para>
        /// 
        /// </summary>
        /// <param name="key"/><param name="defaultValue"/>
        public static float GetFloat( string key, [DefaultValue("0.0F")] float defaultValue ) {
            return EditorPrefs.GetFloat(key, defaultValue);
        }

        /// <summary>
        /// 
        /// <para>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </para>
        /// 
        /// </summary>
        /// <param name="key"/>
        public static float GetFloat( string key ) {
            return EditorPrefs.GetFloat(key);
        }

        /// <summary>
        /// 
        /// <para>
        /// Sets the value of the preference identified by key.
        /// </para>
        /// 
        /// </summary>
        /// <param name="key"/><param name="value"/>
        public static void SetString( string key, string value ) {
            EditorPrefs.SetString(key, value);
        }

        /// <summary>
        /// 
        /// <para>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </para>
        /// 
        /// </summary>
        /// <param name="key"/><param name="defaultValue"/>
        public static string GetString( string key, [DefaultValue("\"\"")] string defaultValue ) {
            return EditorPrefs.GetString(key, defaultValue);
        }

        /// <summary>
        /// 
        /// <para>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </para>
        /// 
        /// </summary>
        /// <param name="key"/>
        public static string GetString( string key ) {
            return EditorPrefs.GetString(key);
        }

        /// <summary>
        /// 
        /// <para>
        /// Sets the value of the preference identified by key.
        /// </para>
        /// 
        /// </summary>
        /// <param name="key"/><param name="value"/>
        public static void SetBool( string key, bool value ) {
            EditorPrefs.SetBool(key, value);
        }

        /// <summary>
        /// 
        /// <para>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </para>
        /// 
        /// </summary>
        /// <param name="key"/><param name="defaultValue"/>
        public static bool GetBool( string key, [DefaultValue("false")] bool defaultValue ) {
            return EditorPrefs.GetBool(key, defaultValue);
        }

        /// <summary>
        /// 
        /// <para>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </para>
        /// 
        /// </summary>
        /// <param name="key"/>
        public static bool GetBool( string key ) {
            return EditorPrefs.GetBool(key);
        }

        /// <summary>
        /// 
        /// <para>
        /// Returns true if key exists in the preferences.
        /// </para>
        /// 
        /// </summary>
        /// <param name="key"/>
        public static bool HasKey( string key ) {
            return EditorPrefs.HasKey(key);
        }

        /// <summary>
        /// 
        /// <para>
        /// Removes key and its corresponding value from the preferences.
        /// </para>
        /// 
        /// </summary>
        /// <param name="key"/>
        public static void DeleteKey( string key ) {
            EditorPrefs.DeleteKey(key);
        }

        /// <summary>
        /// 
        /// <para>
        /// Removes all keys and values from the preferences. Use with caution.
        /// </para>
        /// 
        /// </summary>
        public static void DeleteAll() {
            EditorPrefs.DeleteAll();
        }

        private class ListHelper<T> {
#pragma warning disable 414
            public List<T> list;
#pragma warning restore 414
        }

        public static void SetList<T>( string key, List<T> value ) {
            var val = new ListHelper<T> { list = value };
            var json = JsonUtility.ToJson(val);
            //Debug.LogFormat("SetList json: {0}", json);
            EditorPrefs.SetString(key, json);
        }

        public static List<T> GetList<T>( string key ) {
            var json = EditorPrefs.GetString(key);
            //Debug.LogFormat("GetList json: {0}", json);
            var obj = JsonUtility.FromJson<ListHelper<T>>(json);
            return obj == null ? new List<T>() : obj.list;
        }

        public static void SetClass( string key, System.Object obj ) {

            if ( obj == null ) {
                return;
            }

            var json = JsonUtility.ToJson(obj);

            EditorPrefs.SetString(key, json);
        }
        public static void SetClass( System.Object obj ) {

            if ( obj == null ) {
                return;
            }

            var key = obj.GetType().FullName;
            SetClass(key, obj);
        }

        public static T GetClass<T>( string key ) {
            var json = EditorPrefs.GetString(key);
            return JsonUtility.FromJson<T>(json);
        }

        public static T GetClass<T>() {
            var key = typeof(T).FullName;
            return GetClass<T>(key);
        }

        public static T GetClass<T>( T defaultValue ) {
            var key = typeof(T).FullName;
            return GetClass(key, defaultValue);
        }

        public static T GetClass<T>( string key, T defaultValue ) {
            var obj = GetClass<T>(key);
            return obj != null ? obj : defaultValue;
        }

        public static void SetObject( string key, System.Object obj ) {

            if ( obj == null ) {
                return;
            }

            var realKey = obj.GetType().FullName + ":" + key;
            var json = JsonUtility.ToJson(obj);

            EditorPrefs.SetString(realKey, json);
        }

        public static T GetObject<T>( string key ) {
            var realKey = typeof(T).FullName + ":" + key;
            var json = EditorPrefs.GetString(realKey);
            return JsonUtility.FromJson<T>(json);
        }

        public static T GetObject<T>( string key, T defaultValue ) {
            var obj = GetObject<T>(key);
            return obj != null ? obj : defaultValue;
        }
    }
}