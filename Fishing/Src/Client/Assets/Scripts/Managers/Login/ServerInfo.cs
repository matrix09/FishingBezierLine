using UnityEngine;
using System.Collections.Generic;
using AttTypeDefine;
using SimpleJson;
using System;
using Assets.Scripts.Helpers;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Managers.Login
{
    public class ServerInfo
    {
        public int serverUid;
        public string displayName;
        public int maxUsers;
        public string host;
        public int port;
        public int state = 0;           //0流畅 1繁忙 2拥挤 3维护
        public bool isNew = false;      //是否是“新”服务器
        public int isLastUse = 0;  //0非上次登录，非最近创建角色 1上次登录 2最近创建过角色 3上次登录并且最近创建过角色 
        public bool isRecommend = false;//是否是推荐服务器
        public int roleNum = 0; //拥有的角色数量

        public void Save()
        {
            PlayerPrefs.SetString("ServerHost", host);
            PlayerPrefs.SetInt("ServerPort", port);
        }

        public void Load()
        {
            host = PlayerPrefs.GetString("ServerHost");
            port = PlayerPrefs.GetInt("ServerPort");
        }
    }

    public class NetAddress
    {

        public string host;
        public int port;
        public string name;

        public NetAddress()
        {

        }

        public NetAddress(string h, int p, string n = "")
        {
            host = h;
            port = p;
            name = n.Length != 0 ? n : string.Format("{0}:{1}", h, p);
        }

        public NetAddress(string h, string p, string n = "")
        {
            host = h;
            int.TryParse(p, out port);
            name = n.Length != 0 ? n : string.Format("{0}:{1}", h, p);
        }
    }

    //选择游戏服务器
    public class SelectServer
    {
        public class ServerListInfo
        {
            public List<ServerInfo> servers = new List<ServerInfo>();
        }

        public ServerInfo[] serverList = null;
        public ServerInfo selectServer = null;

        public List<ServerInfo> serverList_Role = new List<ServerInfo>();        //拥有角色
        public List<ServerInfo> serverList_Recommend = new List<ServerInfo>();   //推荐服务器
        public List<ServerListInfo> areaList = new List<ServerListInfo>();      //分区列表
        public int selectArea = -100;    //当前选中的分区 -2推荐服 -1我的服 0~N分区服

    }


    public class SelectPlayerDesc
    {
       // public List<PlayerInfo> roleList = new List<PlayerInfo>();
        //public int selectRoleIndex = 0;
        //public int findItemType = 0;
        //public int findItemSubtype = 0;
        

       


        ////历史接口，暂时没有定好需求，暂留
        //public void Parse(JsonObject data)
        //{
        //    if (data == null)
        //    {
        //        return;
        //    }

        //    roleList.RemoveRange(0, roleList.Count);

        //    JsonArray playerList = (JsonArray)data["playerList"];
        //    for (int i = 0; i < playerList.Count; ++i)
        //    {
        //        JsonArray bb = (playerList[i]).AsArray;
        //        if (bb != null)
        //        {

        //            PlayerInfo tempPlayerInfo = LoginManager.CopyPlayerInfo(bb);
        //            roleList.Add(tempPlayerInfo);
        //        }
        //    }
        //}



     


        //public int GetSelectPlayerID()
        //{
        //    if (selectRoleIndex >= roleList.Count)
        //    {
        //        return 0;
        //    }

        //    return roleList[selectRoleIndex].roleID;
        //}

        //public string GetSelectPlayerName()
        //{
        //    if (selectRoleIndex >= roleList.Count)
        //    {
        //        return "";
        //    }

        //    return roleList[selectRoleIndex].roleName;
        //}

        //public int GetPlayerIndexByName(string inName)
        //{
        //    int count = roleList.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (roleList[i].roleName == inName)
        //        {
        //            return i;
        //        }
        //    }

        //    return -1;
        //}

        //public void RemoveSelect()
        //{
        //    if (selectRoleIndex >= roleList.Count)
        //    {
        //        return;
        //    }

        //    roleList.RemoveAt(selectRoleIndex);
        //    selectRoleIndex = 0;
        //}

        //private ItemInst FindItemCorrectEquip(int type, int subtype)
        //{
        //    findItemType = type;
        //    findItemSubtype = subtype;
        //    return itemList.Find(ItemIsCorrectEquip);
        //}

        //private bool ItemIsCorrectEquip(ItemInst inst)
        //{
        //    if (inst.roleID == GetSelectPlayerID())
        //    {
        //        if (Configuration.Attributes<ItemTemplate>(inst.tempID).ContainsKey(inst.tempID.ToString()))
        //        {
        //            ItemTemplate item = Configuration.Attributes<ItemTemplate>(inst.tempID)[inst.tempID.ToString()];
        //            if (item.itemType == findItemType && item.subType == findItemSubtype)
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        //public ItemTemplate GetSelectRoleEquip_Tou()
        //{
        //    ItemInst inst = FindItemCorrectEquip((int)eItemType.eIT_FangJu, (int)eFangJuSubType.eFJST_Tou);
        //    if (inst == null)
        //    {
        //        return null;
        //    }

        //    if (Configuration.Attributes<ItemTemplate>(inst.tempID).ContainsKey(inst.tempID.ToString()))
        //    {
        //        ItemTemplate item = Configuration.Attributes<ItemTemplate>(inst.tempID)[inst.tempID.ToString()];
        //        return item;
        //    }

        //    return null;
        //}

        //public ItemTemplate GetSelectRoleEquip_Xiong()
        //{
        //    ItemInst inst = FindItemCorrectEquip((int)eItemType.eIT_FangJu, (int)eFangJuSubType.eFJST_Xiong);
        //    if (inst == null)
        //    {
        //        return null;
        //    }

        //    if (Configuration.Attributes<ItemTemplate>(inst.tempID).ContainsKey(inst.tempID.ToString()))
        //    {
        //        ItemTemplate item = Configuration.Attributes<ItemTemplate>(inst.tempID)[inst.tempID.ToString()];
        //        return item;
        //    }

        //    return null;
        //}

        //public ItemTemplate GetSelectRoleEquip_WuQi()
        //{
        //    ItemInst inst = FindItemCorrectEquip((int)eItemType.eIT_WuQi, (int)eWuQiSubType.eWQST_DanShou);
        //    if (inst == null)
        //    {
        //        inst = FindItemCorrectEquip((int)eItemType.eIT_WuQi, (int)eWuQiSubType.eWQST_ShuangChi);
        //    }

        //    if (inst == null)
        //    {
        //        return null;
        //    }

        //    if (Configuration.Attributes<ItemTemplate>(inst.tempID).ContainsKey(inst.tempID.ToString()))
        //    {
        //        ItemTemplate item = Configuration.Attributes<ItemTemplate>(inst.tempID)[inst.tempID.ToString()];
        //        return item;
        //    }

        //    return null;
        //}

        //public int GetSelectRoleSuitID_Enhance()
        //{
        //    if (selectRoleIndex >= roleList.Count)
        //    {
        //        return 0;
        //    }

        //    return roleList[selectRoleIndex].activeEnhanceSuitID;
        //}

        //public int GetSelectRoleSuitID_Inset()
        //{
        //    if (selectRoleIndex >= roleList.Count)
        //    {
        //        return 0;
        //    }

        //    return roleList[selectRoleIndex].activeInsetSuitID;
        //}

        //public int GetSelectRoleFashionID_Weapon()
        //{
        //    return roleList[selectRoleIndex].activeFashionWeaponID;
        //}

        //public int GetSelectRoleFashionID_Equip()
        //{
        //    return roleList[selectRoleIndex].activeFashionEquipID;
        //}
    }

    public class AddressInfo
    {
        public string host = "";
        public int[] port = new int[] { };
        public string name = "";
    }

    public class ServerAddressWWWInfo
    {
        public bool show = false;
        public string message = "";
        public string url = "";
        public IDictionary<string, AddressInfo[]> list;
        //public AddressInfo[] list = new AddressInfo[] { };
    }

}

