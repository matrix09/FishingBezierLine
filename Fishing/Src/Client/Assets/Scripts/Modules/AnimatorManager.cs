using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using Assets.Scripts.Utilities;
namespace Assets.Scripts.Modules
{
    public class AnimatorManager : MonoBehaviour
    {
        public delegate void NotifyFunc(CombatCondition cc);

        private NotifyFunc notifyFunc;

        public void StartAnimation(string str,
                                    NotifyFunc _notify = null,
                                    StateBehaviour.TypeFunc _transBegin = null,
                                    StateBehaviour.TypeFunc _transEnd = null,
                                    StateBehaviour.TypeFunc _animBegin = null,
                                    StateBehaviour.TypeFunc _animEnd = null
                                   )
        {
            Animator am = gameObject.GetComponent<Animator>();

            int hashname = NameHashHelper.StringToHash(str);

            am.SetTrigger(hashname);

            StateBehaviour sb = am.GetBehaviour<StateBehaviour>();

            sb.ClearCallBack();

            sb.RegisterCallBack(StateBehaviour.EventType.TransBegin, _transBegin);

            sb.RegisterCallBack(StateBehaviour.EventType.TransEnd, _transEnd);

            sb.RegisterCallBack(StateBehaviour.EventType.AnimBegin, _animBegin);

            sb.RegisterCallBack(StateBehaviour.EventType.AnimEnd, _animBegin);

            sb.RegisterCallBack(StateBehaviour.EventType.TransEnd, () =>
                {
                    this.InvokeNextFrame(
                        () =>
                        {
                            sb.RegisterCallBack(StateBehaviour.EventType.AnimEnd, _animEnd);
                        }
                    );
                }
            );

            if (_notify != null)
                notifyFunc = _notify;

        }

        public void EventSkillReady ()
        {
            if(null != notifyFunc)
            {
                notifyFunc(CombatCondition.waitingNextSKill);
            }

        }
    }
}
