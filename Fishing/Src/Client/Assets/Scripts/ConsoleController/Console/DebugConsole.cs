using UnityEngine;
using Assets.Scripts.ConsoleController.Console;
using Assets.Scripts.ConsoleController.History;
using Assets.Scripts.ConsoleController.Console.TouchDetectors;
using Assets.Scripts.ConsoleController.Command;
using System;
using TypeDefine;
using System.Runtime.CompilerServices;
using System.Collections;

public class DebugConsole : MonoBehaviour {
    
    //日志视图实例对象
    private LogHistoryView _LogHistoryView;
    private TwoFingerTouchDetector _touchDetector;
    private HelperOverLay _helperOverLay;
    private LogModules _logModules;

    private LogHistory _LogHistory;
    private HeadBar _HeadBar;
    //输入框实例对象
    private InputField _InputField;

    private TitleBar _TitleBar;


    public static bool IsVisible = false;
    private bool bIsVisible = false;
    public bool BIsVisible
    {
        get
        {
            return bIsVisible;
        }
        set
        {

            IsVisible = value;
            if (!bIsVisible && value)
            {
                if(_settings.PauseGameWhenOpen)
                {
                    SystemController.PauseGame();
                }
                bJustBecomeVisible = true;
            }
            else if(bIsVisible && !value)
            {
                if (_settings.PauseGameWhenOpen)
                {
                    SystemController.ResumeGame();
                }
            }

            bIsVisible = value;
        }
    }


    public bool bJustBecomeVisible = false;

    [SerializeField]
    public ImageFilesContainer _imageFiles;
    [SerializeField]
    private Settings _settings;

    public Settings Setting
    {
        get
        {
            return _settings;
        }
        set
        {
            _settings = value;
        }
    }
    public Rect Rect { get; private set; }

    private bool bIsMaximazed = false;
    public bool BIsMaxximazed
    {
        get
        {
            return bIsMaximazed;
        }
    }

    private static DebugConsole _instance;
    public static DebugConsole Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    public class FpsCounter
    {
        // FPS accumulated over the interval
        private float accum;
        public float current = 0.0f;
        private float delta;
        // Frames drawn over the interval
        private int frames = 1;
        // Left time for current interval
        private float timeleft;
        public float updateInterval = 0.5f;

        public FpsCounter()
        {
            timeleft = updateInterval;
        }

        public IEnumerator Update()
        {
            // skip the first frame where everything is initializing.
            yield return null;

            while (true)
            {
                delta = Time.deltaTime;

                timeleft -= delta;
                accum += Time.timeScale / delta;
                ++frames;

                // Interval ended - update GUI text and start new interval
                if (timeleft <= 0.0f)
                {
                    current = accum / frames;
                    timeleft = updateInterval;
                    accum = 0.0f;
                    frames = 0;
                }

                yield return null;
            }
        }
    }


    public FpsCounter fps;




    private void Awake ()
    {
       Initialize();
    }

    private void OnEnable ()
    {
        fps = new FpsCounter();
        StartCoroutine(fps.Update());
    }

    private void OnDisable ()
    {
        Destroy(gameObject);
    }


    //初始化数据
    private void Initialize ()
    {


        //初始化image file
        _imageFiles = (Resources.Load("ConsoleController/Settings/Image_AssetEditor")) as ImageFilesContainer;
        _settings = (Resources.Load("ConsoleController/Settings/Setting_AssetEditor")) as Settings;

        if (_settings.bDontDestroy == true)
        {
            DontDestroyOnLoad(gameObject);
        }



        CommandHandlers.bIsDev = true;

        Instance = this;

        //初始化视图数据
        InitUIElement();

        _touchDetector = new TwoFingerTouchDetector();

        CommandHandlers.CommandExecuted += new Action<CommandHandler>(UpdateRecentCommandsListAfterCommandExecuted);
    }

    private void InitUIElement ()
    {

        _TitleBar = new TitleBar();

        _HeadBar = new HeadBar();

        _LogHistory = new LogHistory(_HeadBar);
        //初始化log视图实例
        _LogHistoryView = new LogHistoryView( _LogHistory);
        //预设命令行UI
        _helperOverLay = new HelperOverLay();

        _logModules = new LogModules( _LogHistoryView, _LogHistory);

      

        //实例键盘实例对象
        _InputField = new KeyBoardInputField( _LogHistoryView, _LogHistory);
    }

    //更新显示近期用过的命令
    private void UpdateRecentCommandsListAfterCommandExecuted (CommandHandler handler)
    {

    }


    private void Update ()
    {
        if(null != _touchDetector)
        {
            _touchDetector.Update();
        }

        foreach (char ch in Setting.OpenAndCloseKeys)
        {
            if(Input.inputString.Contains(ch.ToString()))
            {
                BIsVisible = !BIsVisible;
            
                break;
            }
        }

    }

    private void OnGUI ()
    {
        if(bIsVisible)
        {

            GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.Width((float)Screen.width) };
            GUILayout.BeginVertical(options);

            GUIStyles.BeginCustomSkin(_imageFiles, _settings);

            float consoleHeight = GetConsoleHeight();
            
            Rect = new Rect(0f, 0f, Screen.width, consoleHeight);
            GUI.Window(0x1001, Rect,
                new GUI.WindowFunction(WindowFunc), "",
                GUIStyles.ConsoleWindowBackgroundStyle
              );

            //显示预设命令行UI
            if (_helperOverLay.bIsVisible)
            {
                _helperOverLay.OnGUI(
                    new Rect(0, consoleHeight, Screen.width, Screen.height - consoleHeight)
                    );
            }
            else if (_logModules.BIsVisible)
            {
                _logModules.OnGUI(
                    new Rect(0, consoleHeight, Screen.width, Screen.height - consoleHeight)
                    );
            }
            else
            {

            }

            GUIStyles.EndCustomSkin();

            GUILayout.EndVertical();
        }
    }
    
    //将预设命令放到texteditor
    public void SetCommandLineText (CommandHandler handle)
    {
        _InputField.Input = handle.CommandName;
    }

    private void DrawMainWindow (int windowId, bool inFocus)
    {

            _TitleBar.OnGUI();
            _HeadBar.OnGUI(inFocus);
            //历史log 控制台显示列表
            _LogHistoryView.OnGUI(inFocus);
            //控制台命令输入框
            _InputField.OnGUI();
    }

    private void WindowFunc(int windowId)
    {
        DrawMainWindow(windowId, true);
    }

    //获取console高度
    public float GetConsoleHeight ( )
    {
        float height = Screen.height;

        if (bIsMaximazed == false)
        {
            height *= Setting.fWindowPercent;
        }

        return height;
    }

    public void AddItem (LogHistoryItem item)
    {
        _LogHistory.AddCommandLine(item);
        if(_LogHistory.LogCount >= 1)
            _LogHistoryView.ScrollToShowItem(_LogHistory[_LogHistory.LogCount - 1]);
    }

    public void Minimize ()
    {
        bIsMaximazed = false;
    }

    public void Maximize ()
    {
        bIsMaximazed = true;
    }

    //是显示helpOverLay(预设注册方法)还是显示log module(日志模块开关)
    public void ShowHelpOrModule (bool isShowHelp = true)
    {
        if(isShowHelp == true)
        {
            _helperOverLay.bIsVisible = !_helperOverLay.bIsVisible;
            if (_helperOverLay.bIsVisible)
                _logModules.BIsVisible = false;

        }
        else
        {
            //执行命令行
            _logModules.BIsVisible = !_logModules.BIsVisible;
            if (_logModules.BIsVisible)
                _helperOverLay.bIsVisible = false;
        }
    }

    public LogHistoryLogType GetLogType ()
    {
        return _HeadBar._LogType;
    }

    //删除目前所有的log
    public void CleanLog ()
    {
        _LogHistory.ClearAllLog();
    }



}
