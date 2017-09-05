using UnityEngine;
using System.Collections;

namespace Assets.Scripts.ConsoleController.Console
{

    internal abstract class InputField
    {
        protected InputField () {

        }

        public abstract void ClearInput();

        public abstract void Focus();

        public abstract void LoseFocus();

        public abstract void OnGUI();

        public abstract bool HasFocus { get; }

        public abstract string Input { get; set; }

        public abstract Rect TextFieldRect { get; }

    }

}

