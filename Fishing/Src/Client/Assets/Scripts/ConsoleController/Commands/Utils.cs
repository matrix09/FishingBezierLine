namespace Assets.Scripts.ConsoleController.Command
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Utils
    {
        private static string[] _colorNames = new string[] { "red", "green", "blue", "white", "black", "yellow", "cyan", "magenta", "gray", "clear" };

        public static object GetArgumentValueFromString(string argument, System.Type type)
        {
            if (type == typeof(string))
            {
                return argument;
            }

            if(type == typeof(char))
            {

                string[] _charArr = argument.Split(new char[] {'\"'});
                char[] _charArray = _charArr[0].ToCharArray();
                if(_charArray.Length <= 1)
                {
                   // throw new InvalidProgramException("Invalid Parameter,required parameter type if char");
                    return null;
                }
                else
                {
                    return _charArray[1];
                }
              
            }

            if (type == typeof(int))
            {
                return int.Parse(argument);
            }
            if (type == typeof(float))
            {
                return float.Parse(argument);
            }
            if (type == typeof(bool))
            {
                if (argument == "1")
                {
                    return true;
                }
                if (argument == "0")
                {
                    return false;
                }
                return bool.Parse(argument);
            }
            if (type.IsEnum)
            {
                return Enum.Parse(type, argument, true);
            }
            if (type == typeof(Vector3))
            {
                char[] separator = new char[] { ',' };
                string[] strArray = argument.Split(separator);
                if (strArray.Length == 1)
                {
                    return new Vector3(float.Parse(strArray[0]), float.Parse(strArray[0]), float.Parse(strArray[0]));
                }
                if (strArray.Length != 3)
                {
                    throw new InvalidOperationException("Expected either 1 or 3 arguments.");
                }
                return new Vector3(float.Parse(strArray[0]), float.Parse(strArray[1]), float.Parse(strArray[2]));
            }
            if (type == typeof(Vector2))
            {
                char[] chArray2 = new char[] { ',' };
                string[] strArray2 = argument.Split(chArray2);
                if (strArray2.Length == 1)
                {
                    return new Vector2(float.Parse(strArray2[0]), float.Parse(strArray2[0]));
                }
                if (strArray2.Length != 2)
                {
                    throw new InvalidOperationException("Expected either 1 or 2 arguments.");
                }
                return new Vector2(float.Parse(strArray2[0]), float.Parse(strArray2[1]));
            }

            return null;

        }
      

        public static IEnumerable<string> GetDefaultParameterPossibleOptions(System.Type type)
        {
            List<string> list = new List<string>();
            if (type.IsEnum)
            {
                list.AddRange(Enum.GetNames(type));
                return list;
            }
            if (type == typeof(bool))
            {
                list.Add("true");
                list.Add("false");
                return list;
            }
            if ((type != typeof(Color)) && (type != typeof(Color32)))
            {
                return list;
            }
            return _colorNames;
        }

        public static string GetFriendlyTypeName(System.Type type)
        {
            if (type == typeof(float))
            {
                return "float";
            }
            if (type == typeof(double))
            {
                return "double";
            }
            if (((type == typeof(int)) || (type == typeof(short))) || (type == typeof(long)))
            {
                return "integer";
            }
            if ((type != typeof(Color)) && (type != typeof(Color32)))
            {
                return type.Name;
            }
            return "color";
        }

        public static string WrapInQuotesIfNecessary(string s)
        {
            if (s.Contains(" "))
            {
                return string.Format("\"{0}\"", s);
            }
            return s;
        }
    }
}
