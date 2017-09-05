using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
namespace Assets.Scripts.ConsoleController.Console.TouchDetectors
{

    internal class TwoFingerTouchDetector : TouchDetector
    {

        private const float DRAG_DISTANCE_REQUIRED = 180f;

      
        private float _maxDistanceMoved;
        private Vector2 _OffSet = Vector2.zero;

        private readonly Dictionary<int, Vector2> _initialTouchPositions = new Dictionary<int, Vector2>();
        private readonly Dictionary<int, Vector2> _maximumOffSetPosition = new Dictionary<int, Vector2>();

        
        public TwoFingerTouchDetector ()
        {
            
        }

        private void ResetData (Touch touch)
        {
            _initialTouchPositions.Remove(touch.fingerId);

            _maximumOffSetPosition.Remove(touch.fingerId);
        }
        private void HandleTouchBegin (Touch touch)
        {
            _maximumOffSetPosition.Remove(touch.fingerId);
            _initialTouchPositions[touch.fingerId] = touch.position;   
        }

        private void HandleTouchMoved (Touch touch)
        {
            if(_initialTouchPositions.ContainsKey(touch.fingerId))
            {
                //获取开始位置
                Vector2 vector = _initialTouchPositions[touch.fingerId];
                Vector2 vector2;
                //判断当前最大偏移里面是否存在当前finger id
                if (! _maximumOffSetPosition.TryGetValue(touch.fingerId, out vector2))
                {
                    //如果不存在 ->  当前位置 = 开始位置
                    vector2 = vector;
                }

                //num = 当前位置 - 开始位置
                float num = Vector2.Distance(vector, vector2);

                //如果实际当前位置 > num -> 将实际当前位置赋值给_maximumOffSetPosition 
                if(Vector2.Distance(vector, touch.position) > num)
                {
                    _maximumOffSetPosition[touch.fingerId] = touch.position;
                }          

            }

        }

        private void HandleTouchEnd (Touch touch)
        {
            if(_initialTouchPositions.ContainsKey(touch.fingerId))
            {
                Vector2 vector2;
                Vector2 vector = _initialTouchPositions[touch.fingerId];

                if(!_maximumOffSetPosition.TryGetValue (touch.fingerId, out vector2))
                {
                    vector2 = vector;
                }

                _maxDistanceMoved = Vector2.Distance(vector, vector2);
                _OffSet = vector2 - vector;

                //Debug.Log("HandleTouchEnd");

                DetecSwipeDown(_maxDistanceMoved, _OffSet);
                DetecSwipeUp(_maxDistanceMoved, _OffSet);

                _initialTouchPositions.Remove(touch.fingerId);

                _maximumOffSetPosition.Remove(touch.fingerId);

            }

           




        }

        private void DetecSwipeDown (float maxDistanceMoved, Vector2 offSet)
        {

            //Debug.Log("DetecSwipeDown : maxDistanceMoved" + maxDistanceMoved + "/t" +
            //                    "Input.touchCount " + Input.touchCount + "/t" +
            //                    " DebugConsole.Instance.BIsVisible" + DebugConsole.Instance.BIsVisible
            //    );

            if (
                maxDistanceMoved > DRAG_DISTANCE_REQUIRED &&
                Vector2.Dot(offSet, -Vector2.up) > 0.85 && Input.touchCount == 2 &&
                DebugConsole.Instance.BIsVisible == false
                )
                DebugConsole.Instance.BIsVisible = true;
        }

        private void DetecSwipeUp (float maxDistanceMoved, Vector2 offSet)
        {
         //   Debug.Log("DetecSwipeUp : maxDistanceMoved" + maxDistanceMoved + "/t" +
         //                "Input.touchCount " + Input.touchCount + "/t" +
         //                " DebugConsole.Instance.BIsVisible" + DebugConsole.Instance.BIsVisible
         //);
            if (
                maxDistanceMoved >DRAG_DISTANCE_REQUIRED && 
                Vector2.Dot(offSet, Vector2.up) > 0.85 && 
                Input.touchCount == 2 &&
               DebugConsole.Instance.BIsVisible == true
                )
                DebugConsole.Instance.BIsVisible = false;
        }

        public bool Update()
        {
            bool shouldShowConsole = false;

            //if(null != DebugConsole.Instance && null != DebugConsole.Instance.Setting)
            //{
            //    Debug.Log(DebugConsole.Instance.Setting._eSelectTouchDetector);
            //}
          
            if (null == DebugConsole.Instance || null == DebugConsole.Instance.Setting || DebugConsole.Instance.Setting._eSelectTouchDetector != (Settings.SelectedTouchDetector.TWO_FINGER_SWIPE_DOWN))
                return false;

            for (int i = 0; i < Input.touches.Length; i++)
            {
                Touch touch = Input.touches[i];
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        {
                            HandleTouchBegin(touch);
                            break;
                        }
                    case TouchPhase.Moved:
                        {
                            HandleTouchMoved(touch);
                            break;
                        }
                    //A finger is touching the screen but hasn't moved since the last frame.
                    case TouchPhase.Stationary:
                        {
                            break;
                        }
                        //The system cancelled tracking for the touch, as when (for example) the user puts the device to her face 
                       //or more than five touches happened simultaneously. This is the final phase of a touch.
                    case TouchPhase.Canceled:
                        {
                            ResetData(touch);
                            break;
                        }
                    case TouchPhase.Ended:
                        {
                            HandleTouchEnd(touch);
                            break;
                        }
                }//----end switch

            }//----end for cycle

            return shouldShowConsole;

        }//----end update

    }//----end class

}//----end namespace


