namespace Assets.Scripts.ConsoleController.Command
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    public class ParamInfo
    {
        public MethodInfo AutoCompleteMethod;
        public string[] AutoCompleteOptions;
        public object DefaultValue;
        public bool IsOptional;
        public bool IsParamArray;
        public string Name;
        public Type Type;
    }
}