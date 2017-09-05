using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Helpers;
using UnityEngine;
using UnityEngine.Experimental.Director;
using Assets.Scripts.ConsoleController.Console;

namespace Assets.Scripts.Modules
{
    public class StateBehaviour : StateMachineBehaviour {

        AnimatorStateInfo currentStateInfo;
        AnimatorStateInfo lastStateInfo;
        bool bIsInTransition = false;
        bool bIsLastInTransition = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            currentStateInfo = stateInfo;
            lastStateInfo = stateInfo;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            bIsInTransition = animator.IsInTransition(0);

            currentStateInfo = stateInfo;

            //当前动画播放完毕
            if(!bIsInTransition)
            {
                if(currentStateInfo.normalizedTime%(1.0f) < lastStateInfo.normalizedTime%(1.0f))
                {

                    //停止上一个动画
                    AnimClipEnd();
                    //开始当前的动画
                    AnimClipBegin();
                }
            }

            //动画开始切换
            if(bIsInTransition && !bIsLastInTransition)
            {
                //StateTransBegin
                StateTransBegin();
                //播放AnimBeginClip
                AnimClipBegin();
            }

            //动画切换完毕
            if(!bIsInTransition && bIsLastInTransition)
            {
                //StateTransEnd
                StateTransEnd();
                //播放AnimEndClip
                AnimClipEnd();
            }


            lastStateInfo = stateInfo;
            bIsLastInTransition = bIsInTransition;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
        }

        public enum EventType
        {
            TransBegin,
            TransEnd,
            AnimBegin,
            AnimEnd,
        }
        public delegate void TypeFunc();

        Dictionary<EventType, List<TypeFunc>> delDic = new Dictionary<EventType, List<TypeFunc>>();


        void StateTransBegin ()
        {
            TrigAction(EventType.TransBegin);
        }

        void StateTransEnd ()
        {
            TrigAction(EventType.TransEnd); 
        }

        void AnimClipBegin ()
        {
            TrigAction(EventType.AnimBegin);
        }

        void AnimClipEnd ()
        {
            TrigAction(EventType.AnimEnd);
        }


        void TrigAction (EventType type)
        {
            if(!delDic.ContainsKey(type))
            {
                return;
            }
            List<TypeFunc> lFunc = delDic[type];
            for(int i = 0; i < lFunc.Count; i++)
            {
                if (null == lFunc)
                    continue;
                lFunc[i]();
            }
            lFunc.Clear();
        }

        public void RegisterCallBack (EventType type, TypeFunc func)
        {
            if (null == func)
                return;

            if(!delDic.ContainsKey(type))
            {
                delDic.Add(type, new List<TypeFunc>());
            }

            if(!delDic[type].Contains(func))
            {
                delDic[type].Add(func);
            }
        }

        public void RemoveCallBack (TypeFunc func)
        {

        }

        public void ClearCallBack ()
        {
            delDic.Clear();
        }

        
    }


}
