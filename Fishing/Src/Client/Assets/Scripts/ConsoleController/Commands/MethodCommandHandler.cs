namespace Assets.Scripts.ConsoleController.Command
{
    using System.Reflection;
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    public class MethodCommandHandler : CommandHandler
    {
        private readonly MethodInfo methodInfo;
        private readonly ParamInfo[] _realParameters;
        public MethodInfo _MethodInfo
        {
            get
            {
                return methodInfo;
            }
        }

        //构造
        public MethodCommandHandler(string commandName, object obj, MethodInfo method ) : base(commandName, obj)
        {
            methodInfo = method;
            _realParameters = GenerateParameterList(method);
        }

        //静态接口 ： 获取函数参数
        private static ParamInfo[] GenerateParameterList(MethodInfo methodInfo)
        {
            ParameterInfo[] _params = methodInfo.GetParameters();

            Func<ParameterInfo, ParamInfo> _func = str => TransferToParamInfo(str);

            IEnumerable<ParamInfo> awords = _params.Select(_func);

            return awords.ToArray();
        }

        public static ParamInfo TransferToParamInfo(ParameterInfo x)
        {
            return new ParamInfo {
                IsOptional = x.IsOptional,
                Name = x.Name,
                Type = x.ParameterType,
                DefaultValue = x.DefaultValue,
                //AutoCompleteMethod = CommandHandler.GetAutoCompleteMethod(x),
                IsParamArray = x.GetCustomAttributes(typeof(ParamArrayAttribute), 
                false).Length > 0
            };
        }

        //调用注册接口
        public override bool Invoke(params string[] arguments)
        {
            object[] objArray;
            if (!GetArgumentList(_realParameters, arguments, out objArray))
            {
                return false;
            }
            else if(_objectRef != null)
            {
                methodInfo.Invoke(_objectRef.Target, objArray);
            }
            else
            {
                methodInfo.Invoke(null, objArray);
            }

            return true;
        }
        //获取参数列表
        protected bool GetArgumentList 
        (
            ParamInfo[] paramInfos,
            string[] commandArguments,
            out object[] argumentValues    
        )
        {
            ParamInfo[] infoArray = paramInfos;

            argumentValues = new object[infoArray.Length];

            if(paramInfos.Length != commandArguments.Length)
            {

                Debug.LogWarning("Invalid Parameters");
                return false;
            }
           
            for(int i = 0; i < infoArray.Length; i++)
            {

                ParamInfo info = infoArray[i];
                if(null == (argumentValues[i] = Utils.GetArgumentValueFromString(commandArguments[i], info.Type)))
                {
                    Debug.LogWarning("Invalid Parameter, Ignore Command");
                    return false;
                }
            }
           

            return true;

        }
    }

}


