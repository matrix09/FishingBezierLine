
namespace Assets.Scripts.ConsoleController.Console{

    using UnityEngine;
    using Assets.Scripts.ConsoleController.History;
    using TypeDefine;
    internal class LogHistoryView
    {

        //stack track
        private int _expandedLogHistoryItemIndex = -1;
        private int _expandedItemHeight = 0;
        private string _expandedItemStacktrace = string.Empty;
        //scroll view
        private Vector2 _scrollPosition = Vector2.zero;
        private float _listHeight;
        //event 
        //private float _clickDownTime;
        private Vector2 _clickDownPosition;
        private bool bIsDragging = false;
        private const int LeastDragDis = 25;

        private LogHistory _LogHistory;

        public LogHistoryView(LogHistory _loghistory)
        {
            _LogHistory = _loghistory;
        }

        public void Update() { }


  

        public void OnGUI(bool inFocus)
        {

            GUILayoutOption[] optionArray1 = new GUILayoutOption[] {
                GUILayout.ExpandHeight(true),
                GUILayout.Width((float)Screen.width) };


           _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, optionArray1);

            //绘制log列表
            DrawConsoleItemList(inFocus);

            GUILayout.EndScrollView();


            //Get the rectangle last used by GUILayout for a control.
            Rect lastRect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.Repaint)
            {
                _listHeight = lastRect.height;
            }

            HandleTouchDrag(lastRect);


        }

        bool bChanged = false;
        //绘制log列表
        private void DrawConsoleItemList(bool inFocus)
        {

            _LogHistory.GetHistoryList(out bChanged);
            if(bChanged)
            {
                _expandedLogHistoryItemIndex = -1;
            }

            //统计log数目
            int count = _LogHistory.LogCount;

            //获取scrollview列表高度
            int nListHeight = CalculateListHeight();

            //获取rect
            Rect rect = GUILayoutUtility.GetRect(0f, Screen.width, (float)nListHeight, (float)nListHeight);
            
            //获取列表开头item index
            int scrollFirstItem = (int)(this._scrollPosition.y / ((float)GUIStyles.ConsoleRowHeight));

            //激活扩展显示stacktrack && 
            if (_expandedLogHistoryItemIndex != -1 && scrollFirstItem > _expandedLogHistoryItemIndex )
            {

                if(_scrollPosition.y > _expandedItemHeight)
                {
                    scrollFirstItem = ((int)((_scrollPosition.y - _expandedItemHeight) / (float)GUIStyles.ConsoleRowHeight)) + 1;
                }
                else
                {
                    scrollFirstItem = _expandedLogHistoryItemIndex;
                }

                
            }

            int scrollEndItem = Mathf.Clamp(scrollFirstItem + (Screen.height / GUIStyles.ConsoleRowHeight), 0, count);

            //遍历数据列表
            for (int i = scrollFirstItem; i < scrollEndItem; i++)
            {
                Rect itemRect = new Rect(0f, GetItemTop(i), rect.width, GetItemHeight(i));
                //过滤log
                //if(_LogHistory.CheckFilter (_LogHistory[i]._Type))
                if (_LogHistory[i] == null)
                {
                    Debug.LogException(new System.Exception("===log is empty"));
                    break;
                }
                else
                {

                    DrawConsoleItem(
                                                                  itemRect,
                                                                   _LogHistory[i],
                                                                  i,
                                                                  false
                                                                 );
                }
            }

        }

        //绘制log项
        private void DrawConsoleItem(Rect itemRect, LogHistoryItem historyItem, int index, bool inFocus)
        {
            int controlId = 0x186a0 + index;

            if (itemRect.Contains(Event.current.mousePosition))
            {
                EventType type = Event.current.type;
                if (type == EventType.MouseUp)
                {

                    HandleMouseUpOnItem(itemRect, historyItem, index);
                }
                else
                {
                    //todo
                    HandleMouseDownOnItem(controlId);
                }
            }


            if(Event.current.type == EventType.Repaint)
            {
                if ((index % 2) == 0)
                {
                    GUIStyles.ItemAlternateBackgroundStyle.Draw(itemRect, false, false, false, false);
                }

                if (this._expandedLogHistoryItemIndex == index)
                {
                    this.DrawExpandedItemContent(itemRect, historyItem);
                }
                else
                {
                    this.DrawCollapsedItemContent(itemRect, historyItem);
                }
            }
        }

        private void DrawExpandedItemContent (Rect itemRect, LogHistoryItem _historyItem)
        {

            GUIStyle style;
            Texture2D textured;
            Utils.GetImageAndStyleForHistoryItem(_historyItem, DebugConsole.Instance._imageFiles, out style, out textured);
           
            GUIContent content = new GUIContent(_historyItem._LogMessage);

           

            int num = CalculateRowTextSpaceAvailable(itemRect);

            style.wordWrap = true;

            float num2 = Mathf.Max(style.CalcHeight(content, num), GUIStyles.ConsoleRowHeight);

            Rect rect = new Rect(itemRect.x + textured.width,
                                           itemRect.y,
                                          Screen.width,
                                          num2);
            GUI.Label(rect, content, style);
            GUI.DrawTexture(
                                            new Rect(itemRect.x,
                                                itemRect.y,
                                                textured.width,
                                                GUIStyles.ConsoleRowHeight),
                                            textured,
                                            ScaleMode.ScaleToFit
                                            
                );
            
            if (!string.IsNullOrEmpty(_expandedItemStacktrace))
            {
                rect = new Rect(itemRect.x + textured.width,
                                            itemRect.y + num2,
                                           Screen.width,
                                           _expandedItemHeight - num2);
                GUI.Label(rect, _expandedItemStacktrace, GUIStyles.LogHistoryItemTextAreaStyle);
            }

            style.wordWrap = false;
        }

        private void DrawCollapsedItemContent(Rect itemRect, LogHistoryItem historyItem)
        {

            GUIStyle style;
            Texture2D textured;
            Utils.GetImageAndStyleForHistoryItem(historyItem, DebugConsole.Instance._imageFiles, out style, out textured);
            //style.wordWrap = true;
            GUIContent content = new GUIContent(historyItem._LogMessage);
            Rect rect = new Rect(itemRect.x + textured.width,
                                           itemRect.y,
                                           (float)Screen.width,
                                           (float)GUIStyles.ConsoleRowHeight);

 
            GUI.Label(rect, content, style);
            GUI.DrawTexture(
                               new Rect(itemRect.x,
                                   itemRect.y,
                                  textured.width,
                                   GUIStyles.ConsoleRowHeight),
                               textured,
                               ScaleMode.ScaleToFit

            );
            //style.wordWrap = false;
        }

        private void HandleMouseDownOnItem(int controlId)
        {
            //记录当前被激活的控件
            GUIUtility.hotControl = controlId;
            HandleMobileTouchBeginOnItem();

        }

        private void HandleMobileTouchBeginOnItem ()
        {
            //if(PlatformUtils.IsMobileDevice())
            {
                _clickDownPosition = Event.current.mousePosition;
               // _clickDownTime = Time.realtimeSinceStartup;
               // bIsDragging = false;
            }
        }

        private void HandleTouchDrag (Rect listRect/*control last used by GUILayout*/)
        {
            if(Event.current.isMouse && listRect.Contains(Event.current.mousePosition))
            {
                EventType type = Event.current.type;
                if (type != EventType.MouseUp && type == EventType.MouseDrag)
                {
                    //_totalDragDelta += Event.current.delta.y;
                    bIsDragging = true;
                    _scrollPosition += new Vector2(0, Event.current.delta.y);
                    if (_scrollPosition.y < 0)
                    {
                        _scrollPosition.y = 0;
                    }
                }
            }
          
        }

        private void HandleMouseUpOnItem(Rect itemRect, LogHistoryItem historyItem, int index)
        {
           
            if(bIsDragging || Mathf.Abs(Event.current.mousePosition.y - _clickDownPosition.y) > 5f)
            {
                bIsDragging = false;
            }
            else if (Event.current.button == (int)eMouseType.MouseType_Left)
            {
                /*
               0 means left mouse button
               1 means right mouse button
               2 means middle mouse button
                */
                if (_expandedLogHistoryItemIndex == index)
                {
                    CollapseExpandedItem();
                }
                else
                {
                    ExpandItem(index, itemRect);
                }
            }

            Event.current.Use();

        }

        //收起stack track log
        private void CollapseExpandedItem()
        {
            _expandedLogHistoryItemIndex = -1;
            _expandedItemHeight = 0;
            _expandedItemStacktrace = string.Empty;
            Event.current.mousePosition = new Vector2(-1f, -1f);
        }

        private int CalculateRowTextSpaceAvailable(Rect itemRect)
        {
            //return ((((int)itemRect.width) - (3 * GUIStyles.ConsoleRowTextLeftMargin)) - GUIStyles.ConsoleRowHeight);

            return ((int)itemRect.width);

        }

        //展开stack track log
        private void ExpandItem(int index, Rect _itemRect)
        {
            Texture2D textured;
            GUIStyle style;
            _expandedLogHistoryItemIndex = index;
           
            LogHistoryItem historyItem = _LogHistory[index];
            Utils.GetImageAndStyleForHistoryItem(historyItem, DebugConsole.Instance._imageFiles, out style, out textured);
            style.wordWrap = true;
            int num = this.CalculateRowTextSpaceAvailable(_itemRect);
            float num2 = Mathf.Max(style.CalcHeight(new GUIContent(historyItem._LogMessage), num), GUIStyles.ConsoleRowHeight);
            _expandedItemHeight = (int)num2;
            if (!string.IsNullOrEmpty(historyItem._StackTrace))
            {
                _expandedItemStacktrace = historyItem._StackTrace;
                string[] pasms = _expandedItemStacktrace.Split('\n');
                float height = pasms.Length*GUIStyles.ConsoleRowHeight;// style.CalcHeight(new GUIContent(this._expandedItemStacktrace), (float)Screen.width);
                _expandedItemHeight += (int)(height);
            }
            else
            {
              _expandedItemStacktrace = "";
            }

            style.wordWrap = false;

            ScrollToShowItem(historyItem);

        }

        public void ScrollToShowItem(LogHistoryItem item)
        {
            int index = _LogHistory.GetIndexByItem(item);
            int itemTop = GetItemTop(index);
            int CurItemHeight = itemTop + GetItemHeight(index);
            //float scrollTopPos = _scrollPosition.y;
            float scrollBottomPos = _scrollPosition.y + _listHeight;

            if (CurItemHeight > scrollBottomPos)
            {
                _scrollPosition.y = Mathf.Min(
                                                                    Mathf.Clamp((float)itemTop, 0f, float.PositiveInfinity),
                                                                    Mathf.Clamp(CurItemHeight - _listHeight, 0f, float.PositiveInfinity)
                                                                );
            }
        }

        private int CalculateListHeight()
        {
            int num = _LogHistory.LogCount * GUIStyles.ConsoleRowHeight;
            if (this._expandedLogHistoryItemIndex != -1)
            {
                num +=  this._expandedItemHeight;
            }
            return num;
        }


        private int GetItemTop (int index)
        {
            
            if (this._expandedLogHistoryItemIndex == -1)
            {
                return (index * GUIStyles.ConsoleRowHeight);
            }
            if (index <= this._expandedLogHistoryItemIndex)
            {
                return (index * GUIStyles.ConsoleRowHeight);
            }
            return (((index - 1) * GUIStyles.ConsoleRowHeight) + this._expandedItemHeight);

        }

        private int GetItemHeight(int index)
        {
            if (index != this._expandedLogHistoryItemIndex)
            {
                return GUIStyles.ConsoleRowHeight;
            }
            return _expandedItemHeight;
        }


       


    }
}

