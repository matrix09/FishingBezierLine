

namespace Assets.Scripts.ConsoleController.Command
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Reflection;
    using System;
    using Assets.Scripts.Utilities;
    using System.Linq;
    public static class CommandHandlers
    {
        //DebugConsole
        public static event Action<CommandHandler> CommandExecuted;
        //HelperOverLay
        public static event Action<CommandHandler> CommandHandlerAdded;
        //HelperOverLay
        public static event Action<CommandHandler> CommandHandlerRemoved;

        private static bool bdev = false;

        public static bool bIsDev
        {
            get
            {
                return bdev;
            }
            set
            {
                bdev = value;
            }
        }

        //保存注册实例
        private static Dictionary<string, CommandHandler> _commandHandlers = new Dictionary<string, CommandHandler>();
        //保存类型
        private static HashSet<Type> _registeredCommandHandlers = new HashSet<Type>();
        

        //注册命令
        public static void RegisterCommand(object obj)
        {
            if (!bIsDev) return;

            RegisterCommand(obj.GetType(), obj);
        }

        //注销命令
        public static void UnRegisterCommandHandlers(object obj)
        {
            if (!bIsDev) return;

            if (_registeredCommandHandlers.Contains(obj.GetType()))
            {

                List<string> _list = Enumerable.Select(from i in _commandHandlers
                                                       where (i.Value.WeakObj != null) && (i.Value.WeakObj.Target == obj)
                                                       select i, dd => UnRegister(dd)).ToList();

                for (int i = 0; i < _list.Count; i++)
                {
                    CommandHandlerRemoved(_commandHandlers[_list[i]]);
                    _commandHandlers.Remove(_list[i]);
                }

                _registeredCommandHandlers.Remove(obj.GetType());
            }
        }


        private static void RegisterCommand (Type type,object obj)
        {
         
            if (!_registeredCommandHandlers.Contains(type))
            {
                _registeredCommandHandlers.Add(type);
                RegisterCommandMethod(type, obj);
                RegisterCommandProperty(type, obj);
            }
        }


        private static void RegisterCommandProperty (Type type, object obj)
        {
            PropertyInfo[] properties;

            if(null == obj)
            {
                properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            }
            else
            {
                properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            }

            foreach (PropertyInfo info in properties)
            {
                object[] customAttributes = info.GetCustomAttributes(typeof(CommandLineAttribute), true);

                if(0 != customAttributes.Length)
                {
                    CommandLineAttribute attribute = (CommandLineAttribute)customAttributes[0];

                    string name = attribute.Name;

                    if(string.IsNullOrEmpty (name))
                    {
                        name = info.Name;
                    }
                    RegisterPropertyCommandHandler(type, obj, info, attribute.Description, name);
                }
            }
        }

        private static void RegisterPropertyCommandHandler (Type type, object obj, PropertyInfo property, string description, string commandName)
        {
            string methodOrPropertyName = string.Format("{0}.{1}", property.DeclaringType.FullName, property.Name);

            if (ValidateMethodOrPropertyName(commandName, methodOrPropertyName))
            {

                PropertyCommandHandler handler = new PropertyCommandHandler(commandName, obj, property);
                _commandHandlers.Add(commandName.ToLower(), handler);
                CommandHandlerAdded(handler);

            }

        }


        //注册方法
        private static void RegisterCommandMethod(Type type, object obj)
        {
        
            MethodInfo[] methods;

            if(null == obj)
            {
                methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public|BindingFlags.Static);
            }
            else
            {
                methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            }

            foreach(MethodInfo info in methods)
            {
                object[] customAttributes = info.GetCustomAttributes(typeof(CommandLineAttribute), true);
                if(customAttributes.Length > 0)
                {
                    CommandLineAttribute attribute = (CommandLineAttribute)customAttributes[0];

                    RegisterCommandHandlerMethod(type, obj, info, attribute.Description, attribute.Name);
                }  
            }
        }

        private static void RegisterCommandHandlerMethod(
                                                        Type type, 
                                                        object obj, 
                                                        MethodInfo method, 
                                                        string description, 
                                                        string commandName
                                                        )
        {

            //判断命令名称是否为空
            if(string.IsNullOrEmpty (commandName))
            {
                commandName = method.Name;
            }

            //判断命令名称的有效性
           string methodOrPropertyName = string.Format(
                "{0}.{1}", method.DeclaringType.Name, method.Name
                );
            if(ValidateMethodOrPropertyName
                    (
                        commandName,
                        methodOrPropertyName
                    )
                )
            {
                //实例化方法命令
                MethodCommandHandler handler = new MethodCommandHandler(commandName, obj, method);
                //将方法命令实例添加到_commandHandlers中去
                _commandHandlers.Add(commandName.ToLower(),handler);
                CommandHandlerAdded(handler);
            }
        }

        private static bool ValidateMethodOrPropertyName (string _commandName, string _methodOrPropertyName)
        {

            if(_commandName.Contains(" "))
            {
                Debug.LogWarningFormat("Invalid Command Name:\t{0},Command Name shouldn't contain empty, Ignore!", _commandName);
                return false;
            }

            if(_commandHandlers.ContainsKey(_commandName.ToLower()))
            {
                Debug.LogWarningFormat("{0} already stored, just ignore method:\t{1}", _commandName, _methodOrPropertyName);
                return false;
            }
            return true;

        }

        public static IEnumerable<CommandHandler> Handlers
        {
            get
            {
                return _commandHandlers.Values;
            }
        }

        public static void HandleCommand (string commandline)
        {
            //split string to string[]
            string[] parameters = commandline.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //extention method
            //string[] arguments1 = ArrayUtils.SubArray(parameters, 1, parameters.Length);
            string[] arguments = parameters.SubArray(1, parameters.Length - 1);

            ////通过commandline 找到handler   -> 字符串的切割
            //// DoSomeThing 3, "sdf",'2', 1,2,1
            CommandHandler handler;
            if (!_commandHandlers.TryGetValue(parameters[0].ToLower(), out handler))
            {
                //没有找到相应的命令
                Debug.LogWarningFormat("Invalid Command:\t{0}, Ignore!", parameters[0]);
                return;
            }
            else
            {
                //利用这些信息去完成我们想调用的接口
                if(handler.Invoke(arguments))
                {
                    CommandExecuted(handler);
                }
            }
        }

        private static  string UnRegister(KeyValuePair<string, CommandHandler> i)
        {
            return i.Key;
        }

    }
}


