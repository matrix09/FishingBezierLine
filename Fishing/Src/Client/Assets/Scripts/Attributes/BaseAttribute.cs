using UnityEngine;
using System.Collections;
using AttTypeDefine;
using System.Collections.Generic;

namespace Assets.Scripts.Attributes
{
    public class BaseAttribute : MonoBehaviour
    {
        //玩家绝对阵营
        private ePlayerType playertype;
        public ePlayerType PlayerType
        {
            get
            {
                return playertype;
            }
            set
            {
                if (playertype != value)
                    playertype = value;
            }
        }

        //玩家相对阵营
        private eRelativePlayerType relativeplayertype;
        public eRelativePlayerType RelativePlayerType
        {
            get
            {
                return relativeplayertype;
            }
            set
            {
                if (relativeplayertype != value)
                    relativeplayertype = value;
            }
        }

        //获取基础属性
        protected Dictionary<string, int> baseAtt;
        public int this[eBasicAttribute baType]
        {
            get
            {
                return baseAtt[baType.ToString()];
            }
            set
            {
                if(value != baseAtt[baType.ToString()])
                {
                    baseAtt[baType.ToString()] = value;
                }
            } 
        }

        //获取实际属性


        //获取当前属性

        



        public void InitAtt (Dictionary<string, int> _att)
        {
            baseAtt = _att;
        }

    }

        
}
