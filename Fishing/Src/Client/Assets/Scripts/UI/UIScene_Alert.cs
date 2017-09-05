using System;
using UnityEngine;
using AttTypeDefine;
using Assets.Scripts.Helpers;

public class UIScene_Alert : UIScene {
    public UIButton noBtn;
    public UIButton yesBtn;
    public UIButton quitBtn;
    public UILabel label;
    public UILabel label_Yes;
    public UILabel label_No;
    public UIInput input;
    public GameObject obj_ResetPos;
    public UILabel label_InputPrompt;

    public GameObject rateUI;   //ÆÀ·ÖUI


    private string function;
    private Action<int> action;

    private GameObject obj;

    private bool isQuit = false;

    private int type = -1;
    private bool isYes = true;
    void OnEnable() {
        //GlobeHelper.PlayUIAudio(14001);
        //if(Helper.UIAndActive<UIScene_WebBrowser>()){
        //    Helper.UIAndActive<UIScene_WebBrowser>().CloseBtn();
        //}
    }
    public void InitScene() {
        noBtn.gameObject.SetActive(false);
        yesBtn.gameObject.SetActive(false);
        quitBtn.gameObject.SetActive(false);
        input.gameObject.SetActive(false);
        obj_ResetPos.SetActive(false);
        label_InputPrompt.text = "";

        if (rateUI != null){
            rateUI.SetActive(false);
        }
    }

    public void SetInputPromptInfo(string info) {
       label_InputPrompt.text = info;
    }

    public void SetYesButtonInfo(string info) {
        if (info == "") {
            label_Yes.text = Localization.Get("OK");
        }
        else {
            label_Yes.text = info;
        }
    }

    public void SetNoButtonInfo(string info) {
        if (info == "") {
            label_No.text = Localization.Get("Cancel");
        }
        else {
            label_No.text = info;
        }
    }

    public void SetIsQuit(bool isquit) {
        isQuit = isquit;
    }

    public void SetObj(GameObject obj) {
        this.obj = obj;
    }

    public void SetFunction(string function) {
        this.function = function;
    }

    public void SetType(int alertType, bool yes) {
        this.type = alertType;
        this.isYes = yes;
    }

    public void SetFunction(Action<int> action) {
        this.action = action;
    }

    void DestroyByNo() {
        if (type >= 0) {
            if (!isYes) {
                OpenUI();
            }
            this.type = -1;
        }
        else {
            if (obj && !input.gameObject.activeInHierarchy) {
                obj.SendMessage(function, "2", SendMessageOptions.DontRequireReceiver);
            }
            if (obj && input.gameObject.activeInHierarchy) {
                obj.SendMessage(function, input.text+"Clicked2", SendMessageOptions.DontRequireReceiver);
            }
            if (action != null && !input.gameObject.activeInHierarchy) {
                action.Invoke(2);
            }
        }
       // UIAlert.AddAlertIndex(-1);
        this.gameObject.SetActive(false);
    }

    void DestroyByYes() {
        if (type >= 0) {
            if (isYes) {
                OpenUI();
            }
            this.type = -1;
        }
        else {
            if (obj) {
                if (input.gameObject.activeInHierarchy && label_InputPrompt.text != "") {
                    if (input.text == "")
                    {
                        obj.SendMessage(function, "no", SendMessageOptions.DontRequireReceiver);   
                    }
                    else
                    {
                        obj.SendMessage(function, input.text + "Clicked1", SendMessageOptions.DontRequireReceiver);  
                    }
                     
                }
                else {
                    obj.SendMessage(function, "1", SendMessageOptions.DontRequireReceiver);
                }
            }

            if (action != null && !input.gameObject.activeInHierarchy) {
                action.Invoke(1);
            }
        }
        //UIAlert.AddAlertIndex(-1);
        this.gameObject.SetActive(false);
    }

    void DestroyByQuit() {
        if (type >= 0) {
            this.type = -1;
        }
        else {
            if (obj) {
                obj.SendMessage(function, 0, SendMessageOptions.DontRequireReceiver);
            }

            if (action != null && !input.gameObject.activeInHierarchy) {
                action.Invoke(0);
            }
        }
        Destroy(this.gameObject);
        if (isQuit) {
            Application.Quit();
        }
    }

    void OpenUI() {
        //if (type == (int)eAlertType.eBuy) {
        //    OpenVip();
        //}
        //else if (type == (int)eAlertType.eBag) {
        //    CloseAllUIScene("UIScene_Wealth");
        //    OpenUIByName("UIScene_Bag");
        //}
    }

    void OpenVip() {
        //Helper.Manager<VipAndRechargeManager>().OpenVipAndRechargeByID(0);
    }

    private void ResetPosButtonClicked() {
        Destroy(this.gameObject);

        //if (GlobeHelper.currentLevel != null && GlobeHelper.currentLevel.Inst.levelTarget == (int)eLevelTartet.eLT_SoulPVP)
        //{
        //    return;
        //}
        //GameObject perfectObj = GetPerfectPos();
        //if (perfectObj) {
        //    GlobeHelper.Player.transform.position = perfectObj.transform.position;
        //}
    }

    //private GameObject GetPerfectPos() {


    //    GameObject result = null;
    //    GameObject enemy = GameObject.Find("Enemy");
    //    if (enemy) {
    //        int count = enemy.transform.childCount;
    //        for (int i = 0; i < count; ++i) {
    //            Transform trunk = enemy.transform.FindChild("Trunk_" + (i + 1));
    //            if (trunk != null) {
    //                var npcBc = trunk.GetComponent<NpcBirthControl>();
    //                if (npcBc == null || !npcBc.NpcTrig) {
    //                    continue;
    //                }

    //                for (int j = 0; j < npcBc.transform.childCount; ++j) {
    //                    Transform bo = npcBc.transform.GetChild(j);
    //                    for (int n = 0; n < bo.childCount; ++n) {

    //                        var child = bo.GetChild(n);
    //                        if (child) {
    //                            result = child.gameObject;
    //                            break;
    //                        }
    //                    }

    //                    if (result) {
    //                        break;
    //                    }
    //                }
    //            }

    //            if (result) {
    //                break;
    //            }
    //        }
    //    }

    //    if (!result) {
    //        GameObject startPoint = GameObject.Find("StartPoint");
    //        if (startPoint && startPoint.transform.childCount > 0) {
    //            result = startPoint.transform.GetChild(0).gameObject;
    //        }
    //    }

    //    return result;
    //}

    void NotThanksButtonOnClick() {
        DateTime time =GlobeHelper.GetDateTimeNow();
        int webBrowset_Now = time.Day + time.Month * 30 + time.Year * 360;
        PlayerPrefs.SetInt("NotThanks_Time", webBrowset_Now);
        Destroy(this.gameObject);
    }

    void RateButtonOnClick() {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.cmge.gplay.rod");
        PlayerPrefs.SetString("IsRate","true");
        Destroy(this.gameObject);
    }

    void LaterButtonOnClick() {
        DateTime time =GlobeHelper.GetDateTimeNow();
        long rate_Now = (time.Day + time.Month * 30 + time.Year * 360)*1440 + (time.Hour*60)+time.Minute;
        PlayerPrefs.SetString("MaybeLater_Time",rate_Now.ToString());
        Destroy(this.gameObject);
    }
}
