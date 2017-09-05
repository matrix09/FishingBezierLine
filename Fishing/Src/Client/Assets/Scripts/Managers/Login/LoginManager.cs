

/*

    进入登录服务器

    验证登录结果

    断开服务器

    进入游戏服务器

    账号初始化

    获取角色列表

    创建角色

    获取角色列表

    游戏初始化

    进入场景


    
    游戏服务器地址怎么获取 ？
    答 ：LoginResult :  ls.loginHandler.Login 这条协议会返回游戏服务器的ip和port.

    LoginDataStore存放在哪里：
    答 ： 存放在__ManagersNotDestroy__， 一直保存登录信息.

*/
using SimpleJson;
using UnityEngine;

using System;
using AttTypeDefine;
using Assets.Scripts.Helpers;
namespace Assets.Scripts.Managers.Login
{

    using Assets.Scripts.ConsoleController.Console;
    public class LoginManager
    {
        //进入登录服务器
        public static void EnterLoginServer(string host, int port)
        {
            //DisConnectServer();
            //ServerManager.ServerMgr_LoginConnectorEntry(host, port, LoginSuccess);
        }

    //    //CallBack : LoginServer
    //    private static void LoginSuccess(JsonObject result)
    //    {
    //        if (result.AsInt("result") == (int)eLoginServerResult.Ls_Dispatch)
    //        {
    //            DebugHelper.LogFormat("LoginManager", "Server list Dispatch : Login, json data = {0}", result);
    //            //代表进入服务器，并且转发游戏服务器成功
    //            Helper.Manager<LoginDataStore>()._GameServerAddr.host = result.AsString("host");
    //            Helper.Manager<LoginDataStore>()._GameServerAddr.port = result.AsInt("port");
    //        }
    //        else if (result.AsInt("result") == (int)eLoginServerResult.LoginServer_Success)
    //        {
    //            DebugHelper.LogFormat("LoginManager", "Success : Login, json data = {0}", result);
    //            //验证是否登录成功
    //            CheckLogResult();
    //        }
    //        else
    //        {
    //            UIAlert.Show("Error: Enter Login Server, error code = " + result.AsInt("result"));
    //            return;
    //        }
    //    }

    //    //验证是否登录成功
    //    public static void CheckLogResult()
    //    {
    //        ServerManager.ServerMgr_CheckLoginResult(LoginResult);

    //    }
    //    //CallBack : Login Check Result
    //    private static void LoginResult(JsonObject result)
    //    {
    //        DebugHelper.LogFormat("LoginManager", "Check Login Result :  json data = {0}", result);

    //        //进入登录服务器，数据校验成功
    //        if (result["result"].ToString() == "0")
    //        {

    //            var json = result.ToString();
    //            Helper.Manager<LoginDataStore>().AccountKey = result["key"].ToString();
    //            Helper.Manager<LoginDataStore>().accountID = result["accountID"].ToString();

    //            ServerInfo _ServerInfo = null;
    //            try
    //            {
    //                Helper.Manager<LoginDataStore>()._SelectServer = SimpleJson.SimpleJson.DeserializeObject<SelectServer>(json);
    //            }
    //            catch (Exception ex)
    //            {
    //                Debug.LogWarning(ex.Message);
    //            }
    //            SelectServer selectServer = Helper.Manager<LoginDataStore>()._SelectServer;

    //            //找到推荐服务器，并将推荐服务器作为游戏登录服务器
    //            for (int i = 0; i < selectServer.serverList.Length; i++)
    //            {
    //                ServerInfo si = selectServer.serverList[i];
    //                if (si.isRecommend == true)
    //                {
    //                    _ServerInfo = si;
    //                    break;
    //                }
    //            }

    //            //
    //            Helper.Manager<LoginDataStore>().ServerUid = _ServerInfo.serverUid;
    //            //
    //            Helper.Manager<LoginDataStore>()._GameServerAddr.host = _ServerInfo.host;
    //            //
    //            Helper.Manager<LoginDataStore>()._GameServerAddr.port = _ServerInfo.port;


    //            //进入游戏服务器
    //            EnterGameServer(
    //                Helper.Manager<LoginDataStore>()._GameServerAddr.host,
    //                Helper.Manager<LoginDataStore>()._GameServerAddr.port
    //            );
    //        }
    //        else
    //        {

    //            DebugHelper.ErrorFormat("LoginManager", "Fail to check login result,");
    //        }
    //    }

    //    //断开服务器
    //    public static void DisConnectServer()
    //    {
    //        ServerManager.ServerMgr_DisConnectServer();
    //    }

    //    //进入游戏服务器
    //    public static void EnterGameServer(string host, int port)
    //    {
    //        //登录游戏服务器
    //        ServerManager.ServerMgr_LoginConnectorEntry(host, port, LoginGameServerSuccess);
    //    }

    //    //CallBack : 登录游戏服务器结果
    //    private static void LoginGameServerSuccess(JsonObject result)
    //    {

    //        DebugHelper.LogFormat("LoginManager", "Login Game Server Result :  json data = {0}", result);
    //        //分发服务器，则断开连接，重新登录游戏服务器(服务器返回的json里面包含新的游戏host和port)
    //        if (result.AsInt("result") == (int)eLoginServerResult.Ls_Dispatch)
    //        {
    //            //断开连接
    //            DisConnectServer();

    //            Helper.Manager<LoginDataStore>()._GameServerAddr.host = result.AsString("host");
    //            Helper.Manager<LoginDataStore>()._GameServerAddr.port = result.AsInt("port");
    //            //重新连接游戏服务器
    //            EnterGameServer(
    //               Helper.Manager<LoginDataStore>()._GameServerAddr.host,
    //               Helper.Manager<LoginDataStore>()._GameServerAddr.port
    //           );
    //        }
    //        //游戏服务器连接成功
    //        else if (result.AsInt("result") == (int)eLoginServerResult.GameServer_Success)
    //        {
    //            //初始化账号信息
    //            ServerManager.ServerMgr_InitAccount(InitAccount);
    //        }
    //        else
    //        {
    //            DebugHelper.ErrorFormat("LoginManager", "Fail to enter game server : " + result);
    //        }
    //    }

    //    //账号初始化
    //    public static void InitAccount(JsonObject result)
    //    {
    //        if(result.AsInt("result") == (int)eLoginServerResult.GameServer_Success)
    //        {
    //            //初始化账号成功
    //            Debug.LogFormat("Init Account Result :  json data = {0}", result);

    //            //获取角色列表
    //            ServerManager.ServerMgr_CreateRoleList(GetRoleList);
    //        }
    //        else
    //        {
    //            DebugHelper.ErrorFormat("LoginManager", "InitAccount : result = " + result);
    //        }
         
    //    }

    //    private static void GetRoleList(JsonObject result)
    //    {
    //        if (result.AsInt("result") == (int)eLoginServerResult.GameServer_Success)
    //        {
    //            //获取角色列表
    //            DebugHelper.LogFormat("LoginManager", "Get RoleList Result :  json data = {0}", result);
    //            //如果获取角色列表为空， 则需要创建角色
    //            JsonArray playerList = (JsonArray)result["playerList"];
    //            if (playerList.Count == 0)
    //            {
    //                //创建角色
    //                ServerManager.ServerMgr_CreateRole(CreateRole);
    //            }
    //            //如果角色存在，则直接初始化游戏
    //            else
    //            {
    //                //string info = playerList[0].ToString();
    //                //PlayerInfo playerinfo = SimpleJson.SimpleJson.DeserializeObject<PlayerInfo>(info);
    //                JsonArray rankInfo1 = playerList[0].AsArray;
    //                Helper.Manager<LoginDataStore>().RoleId = rankInfo1[0].AsInt;
    //                //初始化游戏
    //                ServerManager.ServerMgr_InitGame(InitGame);
    //            }
    //        }
    //        else
    //        {
    //            DebugHelper.ErrorFormat("LoginManager", "GetRoleList : result = " + result);
    //        }




    //    }

    //    //创建角色
    //    public static void CreateRole(JsonObject result)
    //    {

    //        if (result.AsInt("result") == (int)eLoginServerResult.GameServer_Success)
    //        {
    //            //创建角色成功
    //            Debug.LogFormat("Create Role Result :  json data = {0}", result);

    //            // 获取角色列表
    //            ServerManager.ServerMgr_CreateRoleList(GetRoleListAfterCreateRole);
    //        }
    //        else
    //        {
    //            DebugHelper.Error("LoginManager", "CreateRole : result = " + result);
    //        }

    //    }

    //    private static void GetRoleListAfterCreateRole(JsonObject result)
    //    {

    //        if (result.AsInt("result") == (int)eLoginServerResult.GameServer_Success)
    //        {
    //            //获取角色列表
    //            DebugHelper.LogFormat("LoginManager", "Get RoleList after create role Result :  json data = {0}", result);

    //            if (result.ContainsKey("playerList"))
    //            {
    //                JsonArray playerList = (JsonArray)result["playerList"];
    //                JsonArray rankInfo1 = playerList[0].AsArray;
    //                Helper.Manager<LoginDataStore>().RoleId = rankInfo1[0].AsInt;

    //            }


    //            //初始化游戏
    //            ServerManager.ServerMgr_InitGame(InitGame);
    //        }
    //        else
    //        {
    //            DebugHelper.ErrorFormat("LoginManager", "GetRoleListAfterCreateRole : result = " + result);
    //        }


        
    //    }


    //    //游戏初始化
    //    public static void InitGame(JsonObject result)
    //    {
    //        if (result.AsInt("result") == (int)eLoginServerResult.GameServer_Success)
    //        {
    //            // 初始化角色成功
    //            DebugHelper.LogFormat("LoginManager", "Init Game Result :  json data = {0}", result);
    //            //初始化阵型
    //            Helper.Manager<FormationManager>().InitResult(result);

    //            //创建主角，赋值属性
    //            Helper.Manager<PlayerListDataStore>().GetMajorInfo(result);

    //            //创建英雄，赋值属性
    //            Helper.Manager<PlayerListDataStore>().GetHerosInfo(result);

    //            //背包装备信息
    //            Helper.Manager<EquipmentManager>().GetEquipmentsInfo(result["bag"].ToString());

    //            //背包材料，消耗品信息
    //            Helper.Manager<WealthManager>().GetWealthsInfo(result["bag"].ToString());

    //            //进入场景
    //            ServerManager.ServerMgr_EnterScene(EnterScene);
    //        }
    //        else
    //        {
    //            DebugHelper.ErrorFormat("LoginManager", "InitGame : result = " + result);
    //        }
    //    }

    //    //进入场景
    //    public static void EnterScene(JsonObject result)
    //    {

    //        if (result.AsInt("result") == (int)eLoginServerResult.GameServer_Success)
    //        {
    //            //进入场景回调
    //            DebugHelper.LogFormat("LoginManager", "Enter Scene Result :  json data = {0}", result);
    //            //进入指定场景
    //            UIHelper.UI<UIScene_Login>().EnterGame();
    //        }
    //        else
    //        {
    //            DebugHelper.ErrorFormat("LoginManager", "EnterScene : result = " + result);
    //        }
    //    }

    //    #region 通用接口
    //    public static PlayerInfo CopyPlayerInfo(JsonObject data)
    //    {
    //        PlayerInfo tempInfo = new PlayerInfo();
    //        if (data.ContainsKey("0"))
    //        {
    //            tempInfo.roleID = GlobeHelper.String2Int(data["0"].ToString());
    //        }
    //        if (data.ContainsKey("1"))
    //        {
    //            tempInfo.accoutID = GlobeHelper.String2Int(data["1"].ToString());
    //        }
    //        if (data.ContainsKey("2"))
    //        {
    //            tempInfo.roleName = data["2"].ToString();
    //        }
    //        if (data.ContainsKey("3"))
    //        {
    //            tempInfo.tempID = GlobeHelper.String2Int(data["3"].ToString());
    //        }
    //        if (data.ContainsKey("4"))
    //        {
    //            tempInfo.expLevel = GlobeHelper.String2Int(data["4"].ToString());
    //        }
    //        if (data.ContainsKey("5"))
    //        {
    //            tempInfo.exp = GlobeHelper.String2Int(data["5"].ToString());
    //        }
    //        if (data.ContainsKey("6"))
    //        {
    //            tempInfo.serverUid = GlobeHelper.String2Int(data["6"].ToString());
    //        }

    //        return tempInfo;
    //    }

    //    public static PlayerInfo CopyPlayerInfo(JsonArray data)
    //    {

    //        PlayerInfo playerInfo = new PlayerInfo();

    //        if (data.Count < 6)
    //        {
    //            return playerInfo;
    //        }

    //        try
    //        {
    //            if (data[0] != null)
    //            {
    //                playerInfo.roleID = data[0].AsInt;
    //            }

    //            if (data[1] != null)
    //            {
    //                playerInfo.accoutID = data[1].AsInt;
    //            }

    //            if (data[2] != null)
    //            {
    //                playerInfo.roleName = data[2].ToString();
    //            }

    //            if (data[3] != null)
    //            {
    //                playerInfo.tempID = data[3].AsInt;
    //            }

    //            if (data[4] != null)
    //            {
    //                playerInfo.expLevel = data[4].AsInt;
    //            }

    //            if (data[5] != null)
    //            {
    //                playerInfo.exp = data[5].AsInt;
    //            }

    //            if (data[6] != null)
    //            {
    //                playerInfo.serverUid = data[6].AsInt;
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.Log("CopyPlayerInfo failed!!!!!!!!" + ex.Message);
    //        }

    //        return playerInfo;
    //    }

    //    #endregion

    //    //加载OtherNotDestroy
    //    public static void LoadOtherNotDestroy()
    //    {

    //        GameObject obj = GameObject.Find("OtherNotDestroy");

    //        if (obj == null)
    //        {
    //            obj = Resources.Load("Prefabs/OtherNotDestroy") as GameObject;
    //            if (obj != null)
    //            {
    //                var managers = MonoBehaviour.Instantiate(obj) as GameObject;
    //                if (managers != null)
    //                    managers.name = "OtherNotDestroy";
    //            }
    //        }
    //        if (null != GlobeHelper.GameMainObj)
    //        {
    //            GlobeHelper.GameMainObj.bChecking = false;
    //        }
    //    }


    }

}
