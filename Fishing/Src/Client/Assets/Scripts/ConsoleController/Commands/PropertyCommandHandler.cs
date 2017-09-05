using UnityEngine;
using System.Collections;
using System.Reflection;
using Assets.Scripts.ConsoleController.History;

namespace Assets.Scripts.ConsoleController.Command
{
    public class PropertyCommandHandler : CommandHandler
    {
        

        private readonly MethodInfo _getMethodInfo;
        private readonly MethodInfo _setMethodInfo;


        public PropertyCommandHandler(string commandName, object obj, PropertyInfo _method) : base(commandName, obj)
        {
           _getMethodInfo = _method.GetGetMethod(true);
           _setMethodInfo = _method.GetSetMethod(true);
        }


        public override bool Invoke(params string[] arguments)
        {

            object obj2;
         //   object[] asd = new object[] {30};


            if (_objectRef != null)
            {
                obj2 = _getMethodInfo.Invoke(WeakObj.Target, arguments);
            }
            else
            {
                obj2 = _getMethodInfo.Invoke(null, null);
            }

            string str = string.Format("{0} : {1}", CommandName, obj2);

            LogHistoryItem item = new LogHistoryItem(TypeDefine.LogHistoryLogType.Log, str, Time.time);
            DebugConsole.Instance.AddItem(item);

            return true;
        }
    }
}



