
namespace Assets.Scripts.ConsoleController.Console
{
    using UnityEngine;
    using Assets.Scripts.ConsoleController.History;
    using TypeDefine;
    using Assets.Scripts.ConsoleController.Command;
    using System.Reflection;
    internal class KeyBoardInputField : InputField
    {
        private string _commandInput = "";
        private LogHistoryView _LogHistoryView;
        private LogHistory _LogHistory;
        private string _inputFieldName;
        private readonly FieldInfo _cursorIndexField;

        public KeyBoardInputField (LogHistoryView _logHistoryView, LogHistory _loghistory)
        {   

            
            _LogHistoryView = _logHistoryView;

            _LogHistory = _loghistory;

     
        }   

        private void HandleKeyDownEvent ()
        {
            KeyCode code = Event.current.keyCode;
            if(code == KeyCode.Return)
            {
                HandleReturnKeyPressed();
            }
            else if(code == KeyCode.UpArrow)
            {
              //  HandleUpKeyPressed();
            }
            else if(code == KeyCode.DownArrow)
            {
             //   HandleDownKeyPressed();
            }
            
        }

        private void HandleReturnKeyPressed ()
        {

            if (0 == _commandInput.CompareTo("")) return;

            //保存command line到loghistoryview中
            LogHistoryItem item = new LogHistoryItem(LogHistoryLogType.ConsoleInput, _commandInput, Time.time);
            _LogHistory.AddCommandLine(item);
         
            //判断_commandInput是否来自预设命令
            CommandHandlers.HandleCommand(_commandInput);

            //清空command line数据
            _commandInput = "";

            Event.current.Use();

            _LogHistoryView.ScrollToShowItem(_LogHistory[_LogHistory.LogCount - 1]);


        }

        //public static IEnumerable<string> SplitCommandLine(string commandLine)
        //{
        //    bool inQuotes = false;
        //    return (from arg in commandLine.Split(' ')
        //            select arg.Trim() into arg
        //            where !string.IsNullOrEmpty(arg)
        //            select arg);


        //}

        public override void OnGUI()
        {

             if (DebugConsole.Instance.GetLogType() != LogHistoryLogType.AllLog)
                return;



              //处理输入框事件
              if(Event.current.type ==  EventType.KeyDown)
               {
                    //处理按下事件
                    HandleKeyDownEvent();
               }


            _inputFieldName = "InputField" + GUIUtility.GetControlID(FocusType.Keyboard);
            GUI.SetNextControlName(this._inputFieldName);

            //确定layout 绘制风格
            GUILayoutOption[] optionArray1 = new GUILayoutOption[] {
                GUILayout.ExpandWidth(true),
                GUILayout.Height(GUIStyles.InputFieldHeight) };

             //确定横向排布
             GUILayout.BeginHorizontal(GUIStyles.HeaderStyle, optionArray1);
            
            //确定text editor绘制风格
            GUILayoutOption[] options = new GUILayoutOption[] {
                GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true) };
            
            //绘制texteditor
            string input = Widgets.FixedSizeTextField(
                _commandInput, 
                GUIStyles.InputTextFieldStyle, 
                options);
        
            _commandInput = input;

            //发送命令行指令
            if(Widgets.HeadBarButton (DebugConsole.Instance._imageFiles._ConsoleInputIcon))
            {
                HandleReturnKeyPressed();
            }

            //弹出显示预设指令UI
            if (Widgets.HeadBarButton(DebugConsole.Instance._imageFiles._HelpIcon))
            {
                //执行命令行
                DebugConsole.Instance.ShowHelpOrModule();
            }

            //弹出log modules模块

            //弹出显示预设指令UI
            if (Widgets.HeadBarButton(DebugConsole.Instance._imageFiles._NextHistoryItemIcon))
            {

                DebugConsole.Instance.ShowHelpOrModule(false);

            }
            if (DebugConsole.Instance.bJustBecomeVisible == true)
            {
                DebugConsole.Instance.bJustBecomeVisible = false;
                _commandInput = "";
                GUI.FocusControl(_inputFieldName);
            }

            GUILayout.EndHorizontal();
        }

        public override bool HasFocus {
            get
            {
              
                return (GUI.GetNameOfFocusedControl() == _inputFieldName);
            }
        }

        public override string Input
        {
            get
            {
                return _commandInput;
            }
            set
            {
                _commandInput = value;
             
            }
        }

        public override Rect TextFieldRect {
            get
            {
                return new Rect(0, 0, 0, 0);
            }
        }


        private void HandleTabKeyPressed()
        {

        }
        private void HandleUpKeyPressed()
        {

        }
        private void HandleDownKeyPressed()
        {

        }

        //LoseFocus
        public override void LoseFocus()
        {

        }
        //Focus
        public override void Focus()
        {
            // TextEditor
        }

        //clear input
        public override void ClearInput()
        {

        }




    }
}