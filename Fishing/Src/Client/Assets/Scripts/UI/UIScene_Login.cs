using UnityEngine;
using Assets.Scripts.ConsoleController.Console;

public class UIScene_Login : UIScene
{
    #region 构造
    void Start()
    {
        BoxCollider[] bcs = gameObject.GetComponentsInChildren<BoxCollider>();

        for(int i = 0; i < bcs.Length; i++)
        {
            BoxCollider bo = bcs[i];

            if(bo.gameObject.name == "Login")
            {
                UIEventListener.Get(bo.gameObject).onClick = PressLogin;
            }
            else if (bo.gameObject.name == "SignIn")
            {
                UIEventListener.Get(bo.gameObject).onClick = PressSignIn;
            }
            else if (bo.gameObject.name == "ForgetPassword")
            {
                UIEventListener.Get(bo.gameObject).onClick = PressForgetPassword;
            }
            else if (bo.gameObject.name == "Send")
            {
                UIEventListener.Get(bo.gameObject).onClick = SendVerifyCode;
            }
            else if (bo.gameObject.name == "Commit")
            {
                UIEventListener.Get(bo.gameObject).onClick = CommitResetPsd;
            }
            else if (bo.gameObject.name == "CommitVerifiyCode")
            {
                UIEventListener.Get(bo.gameObject).onClick = CommitVerifyCode;
            }
            else if (bo.gameObject.name == "BackToLogin")
            {
                UIEventListener.Get(bo.gameObject).onClick = BackToLogin;
            }

            
        }
    }
    #endregion

    #region 登录界面
    public UIWidgetOffect UIWidgetOff_Login;
    void PressForgetPassword(GameObject obj)
    {
        this.LogFormat("Login", "PressForgetPassword");
        UIWidgetOff_Login.TrigMove();
        UIWidgetOff_VerifyCode.TrigMove();
    }

    void PressSignIn(GameObject obj)
    {
        this.LogFormat("Login", "PressSignIn");
    }

    void PressLogin(GameObject obj)
    {
        this.LogFormat("Login", "PressLogin");
        UIWidgetOff_MainPage.TrigMove();
        UIWidgetOff_Login.TrigMove();
    }

    #endregion

    #region 验证界面
    public UILabel UILabel_VerificationCode;
    public UIWidgetOffect UIWidgetOff_VerifyCode;
    void SendVerifyCode(GameObject obj)
    {
        this.LogFormat("Login", "Verify Code");
    }

    void CommitVerifyCode(GameObject obj)
    {
        this.LogFormat("Login", "Commit VerifyCode");

        UIWidgetOff_VerifyCode.TrigMove();

        Invoke("SetPasswordResetUI", 0.6f);

    }

    private void SetPasswordResetUI()
    {
        UIWidgetOff_ResetPsd.TrigMove();
    }


    #endregion

    #region 注册界面

    #endregion

    #region 重置密码
    public UIWidgetOffect UIWidgetOff_ResetPsd;
    void CommitResetPsd(GameObject obj)
    {
        this.LogFormat("Login", "Commit Reset password");
        UIWidgetOff_Login.TrigMove();
        UIWidgetOff_ResetPsd.TrigMove();

    }
    #endregion

    #region 游戏主界面
    public UIWidgetOffect UIWidgetOff_MainPage;

    void BackToLogin (GameObject obj)
    {
        this.LogFormat("Login", "BackToLogin");
        UIWidgetOff_MainPage.TrigMove();
        UIWidgetOff_Login.TrigMove();
    }

    #endregion

    #region 通用接口

    private void SetModelStyle(bool bIsTrue)
    {
        BoxCollider[] bcs = gameObject.GetComponentsInChildren<BoxCollider>();

        for (int i = 0; i < bcs.Length; i++)
        {
            bcs[i].enabled = bIsTrue;
        }
    }
    #endregion
}

/*
   void Start()
   {
       //激活服务器列表
       //ActivateLoginUI(false);

       //初始化UI事件
       InitializeUIEvent();

       //加载场景必要游戏对象
       //LoginManager.LoadOtherNotDestroy();

       //连接策划服务器数据
       ConnectSystemDataStore();

       //刷新服务器列表
       GameCenterRefreshUI();

   }

   //连接策划服务器数据
   private void ConnectSystemDataStore ()
   {
       //SystemDataStore.Instance.Disconnect();
       //SystemDataStore.Instance.Connect("GameDataStore.db");
   }

   //初始化UI事件
   private void InitializeUIEvent()
   {
       List<BoxCollider> boxs = GlobeHelper.GetComponentsAll<BoxCollider>(this.gameObject, true);
       if (boxs != null && boxs.Count > 0)
       {
           for (int j = 0; j < boxs.Count; j++)
           {
               BoxCollider bo = boxs[j];
               if (bo.gameObject.name == "AddAddress")
               {
                   UIEventListener.Get(bo.gameObject).onClick = AddAddress;
               }
               else if(bo.gameObject.name == "ClearServerList")
               {
                   UIEventListener.Get(bo.gameObject).onClick = ClearServerList;
               }

           }
       }
   }

   void ClearServerList (GameObject obj)
   {

       if(bIsTestCode)
       {
           ActivateLoginUI(true);
       }
       else
       {
           PlayerPrefs.DeleteAll();

           var grid = UI<UIGrid>("Connecting.Panel.Grid");

           List<GameObject> lst = new List<GameObject>();
           foreach (Transform child in grid.transform)
           {
               lst.Add(child.gameObject);

           }

           for (int i = 0; i < lst.Count; i++)
           {
               if (lst[i].gameObject.name == "Button0") continue;
               Destroy(lst[i].gameObject);
           }

           LoginDataStore _loginDataStore = Helper.Manager<LoginDataStore>();

           _loginDataStore._LoginServerList.Clear();

           grid.gameObject.SetActive(false);
       }


   }

   //增加服务器列表地址
   void AddAddress(GameObject obj)
   {
       LoginDataStore _loginDataStore = Helper.Manager<LoginDataStore>();

       var hostport = UI<UIInput>("Connecting.Input").value.Split(':');
       if (hostport.Length != 2)
       {
           UIAlert.Show("The address is invalid, please use the format: 192.168.21.113:17001");
           return;
       }

       //判断是否服务器地址已经存在
       var newaddr = new NetAddress(hostport[0], hostport[1]);
       foreach (var netAddress in _loginDataStore._LoginServerList)
       {
           if (newaddr.name == netAddress.name)
           {
               UIAlert.Show(string.Format("Address {0} is already added.", newaddr.name));
               return;
           }
       }

       //保存新进的服务器地址
       var i = 0;
       while (true)
       {
           if (PlayerPrefs.HasKey(string.Format("NetAddress{0}", i)))
           {
               ++i;
               continue;
           }
           PlayerPrefs.SetString(string.Format("NetAddress{0}", i), newaddr.name);
           PlayerPrefs.Save();
           break;
       }


       _loginDataStore._LoginServerList.Add(newaddr);
       //刷新服务器列表UI
       GameCenterRefreshUI();

   }

   //刷新服务器列表
   private void GameCenterRefreshUI()
   {
       LoginDataStore loginData = Helper.Manager<LoginDataStore>();

       Debug.Assert(null != loginData);

       int count = loginData._LoginServerList.Count;
       //Math.Max(ServerListGrid.transform.childCount, loginData._LoginServerList.Count);
       var grid = UI<UIGrid>("Connecting.Panel.Grid");
       var button = UI<UIButton>("Connecting.Panel.Grid.Button0");
       if (count == 0)
       {
           loginData._LoginServerList.AddRange(loginData.address);
           loginData.LoadGameCenterConfig();
       }


       count = loginData._LoginServerList.Count;

       if(count != 0)
           grid.transform.gameObject.SetActive(true);

       for (int i = 0; i < count; ++i)
       {
           if (i < loginData._LoginServerList.Count)
           {
               var ui = UI<UILabel>(string.Format("Connecting.Panel.Grid.Button{0}.Label", i));
               if (ui == null)
               {
                   var btn = Instantiate(button.gameObject) as GameObject;
                   if (btn != null)
                   {
                       btn.name = string.Format("Button{0}", i);
                       btn.transform.parent = grid.gameObject.transform;
                       Vector3 v3 = new Vector3();
                       v3 = btn.transform.localPosition;
                       v3.z = 0;
                       btn.transform.localPosition = v3;
                       btn.transform.localScale = Vector3.one;
                       UIEventListener.Get(btn).onClick = PressSelectServer;
                       //btn.transform.position = Vector3.zero;
                   }
                   ui = UI<UILabel>(string.Format("Connecting.Panel.Grid.Button{0}.Label", i));
               }
               else
               {
                   UIEventListener.Get(button.gameObject).onClick = PressSelectServer;
               }

               ui.text = loginData._LoginServerList[i].name;
               //countServer++;
               // Show all address in editor and not in real device.
               if (!Application.platform.ToString().Contains("Editor"))
               {
                   if (ui.text.StartsWith("192==") || ui.text.StartsWith("188=="))
                   {
                       SetButtonGray(ui.transform.parent.gameObject, true);
                      // countServer--;
                   }
                   else {
                       SetButtonGray(ui.transform.parent.gameObject, false);
                   }
               }

           }
       }

       grid.Reposition();
   }

   //选中游戏登录服务器
   private NetAddress _SelectCenter;
   public void PressSelectServer (GameObject sender)
   {
       LoginDataStore loginData = Helper.Manager<LoginDataStore>();
       int index = 0;
       const string baseName = "Button";

       if (!int.TryParse(sender.name.Substring(baseName.Length, sender.name.Length - baseName.Length), out index))
       {
           this.Log("Login", "ConnectCenterClicked TryParse failed:" + sender.name);
           return;
       }

       if (index < loginData._LoginServerList.Count)
       {
           _SelectCenter = loginData._LoginServerList[index];
           //ProcessEvent(Event.SignIn);
           ActivateLoginUI(true);
       }
   }

   //激活登录UI/服务器列表UI
   private void ActivateLoginUI(bool isOpen)
   {
       ConnectingObj.SetActive(!isOpen);
       SignInObj.SetActive(isOpen);
   }

   //点击登陆按钮,进入游戏
   private void OnPressLogin(GameObject obj)
   {
       LoginManager.EnterLoginServer(_SelectCenter.host, _SelectCenter.port);
   }

   //进入主城
   public void EnterGame()
   {
       //加载资源->进入剧情场景20001
       //Helper.Manager<RoomManager>().EnterSceneById((int)AttTypeDefine.eSceneIndex.nMap_Main);
       //销毁登陆UI
       //Destroy(gameObject);
   }
   */
