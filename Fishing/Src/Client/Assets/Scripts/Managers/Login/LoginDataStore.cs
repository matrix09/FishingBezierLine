using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Assets.Scripts.Managers.Login
{
    //挂接到__ManagersNotDestroy__
    public class LoginDataStore : MonoBehaviour
    {
        //校验ID
        private string checkid = "";
        public string CheckId
        {
            get
            {
                if (checkid.Length == 0)
                {
                    checkid = SystemInfo.deviceUniqueIdentifier;
                
                }

                return checkid;
            }
        }
        //账号ID
        public string accountID;
        //账号KEY
        public string AccountKey;
        //服务器唯一标示ID
        public int ServerUid;
        //角色ID
        public int RoleId;
        //角色列表 服务器协议 ： ps.loginHandler.GetRoleList.  接口名称 ： GetRoleListAfterCreateRole.
        public readonly SelectPlayerDesc mSelectPlayer = new SelectPlayerDesc();
        //游戏服务器列表
        public SelectServer _SelectServer = new SelectServer();
        //登录服务器
        private NetAddress netaddr_loginserver = new NetAddress();
        public NetAddress _LoginServerAddr
        {
            get
            {
                return netaddr_loginserver;
            }
            private set
            {
                netaddr_loginserver = value;
            }
        }
        //登录服务器列表
        private List<NetAddress> _loginserverlist = new List<NetAddress>();
        public List<NetAddress> _LoginServerList
        {
            get
            {
                return _loginserverlist;
            }
            //set
            //{
            //    _loginserverlist = value;
            //}
        }

        public  List<NetAddress> address = new List<NetAddress>
        {
            //"test.qqlogin.qmphs.qq.com", 50071    
            //new NetAddress("test.qqlogin.qmphs.qq.com", 50071, Localization.instance.Get("LoginServerName0")),
            //new NetAddress("test.qqlogin.qmphs.qq.com", 50061, Localization.instance.Get("LoginServerName0")),
            //new NetAddress("aqqlogin.qmphs.qq.com", 10081, Localization.instance.Get("LoginServerName4")),
            new NetAddress("188.188.0.187", 26001, "188.188.0.187:26001"),
             //new NetAddress("192.168.21.230", 26001, "192.168.21.230"),
            // new NetAddress("203.195.143.239", 26001, Localization.instance.Get("LoginServerName1")),
            // new NetAddress("aqqlogin.qmphs.qq.com", 10081, Localization.instance.Get("LoginServerName3")),
            //new NetAddress("test.qqlogin.qmphs.qq.com", 50061, "test.qqlogin.qmphs.qq.com"),
            //new NetAddress("203.195.161.167", 27001, "203.195.161.167"),
            // new NetAddress("188.188.0.170", 17001, Localization.instance.Get("LoginServerName2")),
            // new NetAddress("218.241.144.114", 42094),
            //new NetAddress("192.168.21.35", 26001),
            //new NetAddress("188.188.0.162", 17001),
        };


        //游戏服务器
        private NetAddress netaddr_gameserver = new NetAddress();
        public NetAddress _GameServerAddr
        {
            get
            {
                return netaddr_gameserver;
            }
            private set
            {
                netaddr_gameserver = value;
            }
        }


        public  void LoadGameCenterConfig()
        {
            var i = 0;
            while (true)
            {
                if (!PlayerPrefs.HasKey(string.Format("NetAddress{0}", i++)))
                {
                    break;
                }

                var result = PlayerPrefs.GetString(string.Format("NetAddress{0}", i));
                var hostport = result.Split(':');
                if (hostport.Length != 2)
                {
                    continue;
                }

                var newaddr = new NetAddress(hostport[0], hostport[1]);

                foreach (var netAddress in _loginserverlist)
                {
                    if (newaddr.name == netAddress.name)
                    {
                        newaddr.name = "";
                        break;
                    }
                }
                if (newaddr.name.Length > 0)
                {
                    _loginserverlist.Add(newaddr);
                }
            }
        }

    }
}

