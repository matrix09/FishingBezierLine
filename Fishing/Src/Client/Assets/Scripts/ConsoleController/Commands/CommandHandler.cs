using UnityEngine;
using System.Collections;
using System;

namespace Assets.Scripts.ConsoleController.Command
{
    public abstract class CommandHandler
    {
        //内部变量
        protected string _commandName;
        protected WeakReference _objectRef;

        protected CommandHandler (string commandName, object obj) 
        {
            _commandName = commandName;

            if(null != obj)
            {
                _objectRef = new WeakReference(obj);
            }

        }

        //public CommandHandler() { }


        //公有变量
        public string CommandName
        {
            get
            {
                return _commandName;
            }
        }
        public WeakReference WeakObj
        {
            get
            {
                return _objectRef;
            }
        }

        public abstract bool Invoke(params string[] arguments);




    }
}
