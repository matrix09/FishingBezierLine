using Assets.Scripts.ConsoleController.Console;
using System.Runtime.CompilerServices;
using UnityEngine;
using Assets.Scripts.Utilities;
public static class GUIStyles {


  
    //styles
    private static GUIStyle itemAlternateBackgroundStyle;
    
    //error
    private static GUIStyle errorLabelStyle;
    //warning
    private static GUIStyle warningLabelStyle;
    //exception
    private static GUIStyle exceptionLabelStyle;
    //info
    private static GUIStyle infoLabelStyle;
    //assert
    private static GUIStyle assertLabelStyle;

    //25:back ground style
    private static GUIStyle consoleWindowBackgroundStyle;
    //26: help window background style
    //private static GUIStyle helpWindowBackGroundStyle;
    public static GUIStyle HelpWindowBackGroundStyle{get; set;}
    public static GUIStyle LogModulesBackGroundStyle { get; set; }
    public static float SuggestionButtonHeight;

    //input style
    private static GUIStyle inputTextFieldStyle;

    private static GUIStyle headerStyle;

    //GUI 自定义皮肤
    private static GUISkin _customGUISkin;
    //GUI 默认皮肤
    private static GUISkin _defaultGUISkin;
    //标题高度
    private static float titleHeight;
    public static float TitleHeight
    {

        get
        {
            return titleHeight;
        }

        private set
        {
            titleHeight = value;
        }
    }
    //标签高度
    private static float headerHeight;
    public static float HeaderHeight
    {

        get
        {
            return headerHeight;
        }

        private set
        {
            headerHeight = value;
        }
    }
    //每行log高度
    private static int consoleRowHeight = 0;
    public static int ConsoleRowHeight
    {
         
        get
        {
            return consoleRowHeight;
        }
         
        private set
        {
            consoleRowHeight = value;
        }
    }
    //命令行高度
    private static float inputfieldHeight;
    public static float InputFieldHeight
    {

        get
        {
            return inputfieldHeight;
        }

        private set
        {
            inputfieldHeight = value;
        }
    }
    private static GUIStyle headbuttonlabelstyle;
    public static GUIStyle HeaderButtonLabelStyle
    {
         
        get
        {
            return headbuttonlabelstyle;
        }
         
        private set
        {
            headbuttonlabelstyle = value;
        }
    }

    private static GUIStyle logHistoryItemTextAreaStyle;
    public static GUIStyle LogHistoryItemTextAreaStyle
    {
         
        get
        {
            return logHistoryItemTextAreaStyle;
        }
         
        private set
        {
            logHistoryItemTextAreaStyle = value;
        }
    }

    public static GUIStyle InputTextFieldStyle
    {
         
        get
        {
            return inputTextFieldStyle;
        }
         
        private set
        {
            inputTextFieldStyle = value;
        }
    }

    public static GUIStyle TitleBarFieldStyle;



    public static GUIStyle HeaderStyle
    {
         
        get
        {
            return headerStyle;
        }
         
        private set
        {
            headerStyle = value;
        }
    }


    public static GUIStyle ItemAlternateBackgroundStyle
    {
         
        get
        {
            return itemAlternateBackgroundStyle;
        }
         
        private set
        {
            itemAlternateBackgroundStyle = value;
        }
    }
    //error
    public static GUIStyle ErrorLabelStyle
    {
         
        get
        {
            return errorLabelStyle;
        }
         
        private set
        {
            errorLabelStyle = value;
        }
    }
    //warning
    public static GUIStyle WarningLabelStyle
    {
         
        get
        {
            return warningLabelStyle;
        }
         
        private set
        {
            warningLabelStyle = value;
        }
    }
    //exception
    public static GUIStyle ExceptionLabelStyle
    {
         
        get
        {
            return exceptionLabelStyle;
        }
         
        private set
        {
            exceptionLabelStyle = value;
        }
    }
    //info
    public static GUIStyle InfoLabelStyle
    {
         
        get
        {
            return infoLabelStyle;
        }
         
        private set
        {
            infoLabelStyle = value;
        }
    }
    //assert
    public static GUIStyle AssertLabelStyle
    {
         
        get
        {
            return assertLabelStyle;
        }
         
        private set
        {
            assertLabelStyle = value;
        }
    }

    //back ground style
    public static GUIStyle ConsoleWindowBackgroundStyle
    {
         
        get
        {
            return consoleWindowBackgroundStyle;
        }
         
        private set
        {
            consoleWindowBackgroundStyle = value;
        }
    }

 

    public static void BeginCustomSkin (ImageFilesContainer imageFiles, Settings settings)
    {
        
        SetUpStyles(imageFiles, settings, false);
        //保存原始gui skin
        _defaultGUISkin = GUI.skin;
        //todo zxf
        GUI.skin = _customGUISkin;

    }

    public static void EndCustomSkin()
    {
        GUI.skin = _defaultGUISkin;
    }


    public static void SetUpStyles (ImageFilesContainer imageFiles, Settings settings, bool force = false)
    {
        if (null == _customGUISkin || force)
        {
            //todo zxf
            //每一栏的高度
            ConsoleRowHeight = 20;

            TitleHeight = 15;
            HeaderHeight = 20;
            InputFieldHeight = 20;


            //字体高度
            int otherfontsize = 18;
            //滚动条的宽度
            int scrollbarThumbWidth = 40;

            _customGUISkin = ScriptableObject.CreateInstance<GUISkin>();

            _customGUISkin.font = GUI.skin.font;

            _customGUISkin.horizontalScrollbar = new GUIStyle(GUI.skin.horizontalScrollbar);
            _customGUISkin.horizontalScrollbarLeftButton = new GUIStyle(GUI.skin.horizontalScrollbarLeftButton);
            _customGUISkin.horizontalScrollbarRightButton = new GUIStyle(GUI.skin.horizontalScrollbarRightButton);
            _customGUISkin.horizontalScrollbarThumb = new GUIStyle(GUI.skin.horizontalScrollbarThumb);
            _customGUISkin.horizontalSlider = new GUIStyle(GUI.skin.horizontalSlider);
            _customGUISkin.horizontalSliderThumb = new GUIStyle(GUI.skin.horizontalSliderThumb);

            SuggestionButtonHeight = 100;

            GUIStyle style2 = new GUIStyle(GUI.skin.label)
            {
                fontSize = otherfontsize
            };

            _customGUISkin.label = style2;


            _customGUISkin.scrollView = new GUIStyle(GUI.skin.scrollView);

            GUIStyle style3 = new GUIStyle(GUI.skin.textArea)
            {
                fontSize = otherfontsize
            };

            _customGUISkin.textArea = style3;
            GUIStyle style4 = new GUIStyle(GUI.skin.textField)
            {
                fontSize = otherfontsize
            };

            _customGUISkin.textField = style4;
            GUIStyle style5 = new GUIStyle(GUI.skin.toggle)
            {
                fontSize = otherfontsize
            };

            _customGUISkin.toggle = style5;
            GUIStyle style6 = new GUIStyle(GUI.skin.verticalScrollbar)
            {
                fixedWidth = scrollbarThumbWidth
            };

            //HeaderHeight = ConsoleRowHeight;

            _customGUISkin.verticalScrollbar = style6;
            _customGUISkin.verticalScrollbarDownButton = new GUIStyle(GUI.skin.verticalScrollbarDownButton);
            _customGUISkin.verticalScrollbarThumb = new GUIStyle(GUI.skin.verticalScrollbarThumb);
            _customGUISkin.verticalScrollbarUpButton = new GUIStyle(GUI.skin.verticalScrollbarUpButton);
            _customGUISkin.verticalScrollbarThumb.fixedWidth = scrollbarThumbWidth;
            _customGUISkin.verticalSlider = new GUIStyle(GUI.skin.verticalSlider);
            _customGUISkin.verticalSliderThumb = new GUIStyle(GUI.skin.verticalSliderThumb);
            _customGUISkin.window = new GUIStyle(GUI.skin.window);
            SetBackgroundForAllStyleStates(_customGUISkin.verticalScrollbarThumb, imageFiles._ScrollbarThumb);

            GUIStyle other = new GUIStyle(GUI.skin.label)
            {
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                fontSize = otherfontsize,
                wordWrap = false,
                alignment = TextAnchor.MiddleLeft
            };

        
            //error
            GUIStyle style10 = new GUIStyle(other)
            {
                normal = { textColor = Color.red }
            };
            ErrorLabelStyle = style10;
            //warning
            GUIStyle style11 = new GUIStyle(other)
            {
                normal = { textColor = Color.yellow }
            };
            WarningLabelStyle = style11;
            //assert
            GUIStyle style12 = new GUIStyle(other)
            {
                normal = { textColor = Color.green }
            };
            AssertLabelStyle = style12;
            //exception
            GUIStyle style13 = new GUIStyle(other)
            {
                normal = { textColor = new Color32(0x30, 0xa3, 0xff, 0xff) }
            };
            ExceptionLabelStyle = style13;
            //info
            GUIStyle style14 = new GUIStyle(other)
            {
                normal = { textColor = Color.white }
            };
            InfoLabelStyle = style14;

            //back ground
            GUIStyle style25 = new GUIStyle("box")
            {
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0),
                normal = { background = UIUtilities.CreateTexture(new Color(0.15f, 0.15f, 0.15f, 0.9f)) },
                fontSize = otherfontsize
            };
            ConsoleWindowBackgroundStyle = style25;

           
            GUIStyle style26 = new GUIStyle(ConsoleWindowBackgroundStyle)
            {
                padding = new RectOffset((int)(1 * 50f), (int)(1 * 50f), 0, 0),
                normal = { background = UIUtilities.CreateTexture(new Color(0.278f, 0.278f, 0.278f, 1f)) }
            };
            HelpWindowBackGroundStyle = style26;


            GUIStyle style27 = new GUIStyle(ConsoleWindowBackgroundStyle)
            {
                padding = new RectOffset((int)(1 * 50f), (int)(1 * 50f), 0, 0),
                normal = { background = UIUtilities.CreateTexture(new Color(0.278f, 0.278f, 0.278f, 1f)) }
            };
            LogModulesBackGroundStyle = style27;

            GUIStyle style28 = new GUIStyle
            {
                normal = { background = UIUtilities.CreateTexture(new Color(0f, 0f, 0f, 0.1f)) }
            };
            ItemAlternateBackgroundStyle = style28;
            GUIStyle style31 = new GUIStyle
            {
                normal = { background = imageFiles._BackgroundGradient }
            };
            HeaderStyle = style31;

            GUIStyle style38 = new GUIStyle(GUI.skin.textField)
            {
                alignment = TextAnchor.MiddleLeft,
                padding = new RectOffset((int)(10f * 1), (int)(10f * 1), 2, 2),
                fontSize = (int)(40 * 0.5f),
                margin = new RectOffset(5, 3, 3, 4)
            };
            InputTextFieldStyle = style38;

            GUIStyle style40 = new GUIStyle(GUI.skin.label)
            {
                fontSize = (int)(16),
                padding = new RectOffset(),
                margin = new RectOffset(),
                richText = true,
                alignment = TextAnchor.MiddleLeft,
                hover = {
                background = UIUtilities.CreateTexture(new Color(0f, 0f, 0.6f, 0.4f)),
                textColor = Color.white
            }
            };

            GUIStyle style39 = new GUIStyle(GUI.skin.textField)
            {
                alignment = TextAnchor.MiddleCenter,
                //padding = new RectOffset((int)(10f * 1), (int)(10f * 1), 2, 2),
                fontSize = (int)(20 * 0.5f),
                //margin = new RectOffset(5, 3, 3, 4)
            };


            TitleBarFieldStyle = style39;

            LogHistoryItemTextAreaStyle = style40;


            GUIStyle style61 = new GUIStyle(other)
            {
                stretchHeight = true,
                padding = new RectOffset(5, 5, 3, 3),
                fontSize = (int)(HeaderHeight * 0.5f)
            };
            HeaderButtonLabelStyle = style61;


        }
        

    }


    private static void SetBackgroundForAllStyleStates(GUIStyle style, Texture2D texture)
    {
        style.normal.background = texture;
        style.hover.background = texture;
        style.active.background = texture;
        style.onNormal.background = texture;
        style.onHover.background = texture;
        style.onActive.background = texture;
        style.focused.background = texture;
        style.onFocused.background = texture;
    }



}
