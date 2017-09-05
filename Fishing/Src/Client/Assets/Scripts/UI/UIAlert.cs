using UnityEngine;
using System;
using AttTypeDefine;
using Assets.Scripts.Helpers;

public class UIAlert : MonoBehaviour {
    public static void Show(string info, string yesButtonInfo = "") {
        UIScene_Alert alertView = createAlertView();
        if (!alertView) {
            return;
        }
        alertView.yesBtn.gameObject.SetActive(true);
        alertView.label.text = info;

        alertView.SetYesButtonInfo(yesButtonInfo);

        alertView.yesBtn.transform.localPosition = alertView.quitBtn.transform.localPosition;
    }

    public static void ShowYes(string info, GameObject Obj, string function, string yesButtonInfo = "") {
        UIScene_Alert alertView = createAlertView();
        if (!alertView) {
            return;
        }
        alertView.yesBtn.gameObject.SetActive(true);
        alertView.label.text = info;

        alertView.SetObj(Obj);
        alertView.SetFunction(function);
        alertView.SetYesButtonInfo(yesButtonInfo);

        alertView.yesBtn.transform.localPosition = alertView.quitBtn.transform.localPosition;
    }

    public static void Show(string info, Action<int> action, string yesButtonInfo = "",
                            string noButtonInfo = "",bool showResetPos = false) {
        UIScene_Alert alertView = createAlertView();
        if (!alertView) {
            return;
        }
        alertView.noBtn.gameObject.SetActive(true);
        alertView.yesBtn.gameObject.SetActive(true);
        alertView.label.text = info;

        Vector3 pos = alertView.noBtn.transform.localPosition;
        pos.x = -pos.x;
        alertView.yesBtn.transform.localPosition = pos;

        alertView.SetYesButtonInfo(yesButtonInfo);
        alertView.SetNoButtonInfo(noButtonInfo);

        alertView.SetFunction(action);
        //if (GlobeHelper.currentLevel != null && GlobeHelper.currentLevel.Inst.levelTarget == (int)eLevelTartet.eLT_TeamDrak)
        //{
        //    alertView.obj_ResetPos.SetActive(false);
        //}
        //else {
        //    alertView.obj_ResetPos.SetActive(showResetPos);
        //}
    }

    //isYes 是否点yes按钮跳转
    public static void Show_AlertType(string info,int type,bool isYes = true,string yesButtonInfo = "",
                        string noButtonInfo = "") {
        UIScene_Alert alertView = createAlertView();
        if (!alertView) {
            return;
        }
        alertView.noBtn.gameObject.SetActive(true);
        alertView.yesBtn.gameObject.SetActive(true);
        alertView.label.text = info;

        Vector3 pos = alertView.noBtn.transform.localPosition;
        pos.x = -pos.x;
        alertView.yesBtn.transform.localPosition = pos;

        alertView.SetYesButtonInfo(yesButtonInfo);
        alertView.SetNoButtonInfo(noButtonInfo);

        alertView.SetType(type,isYes);
    }

    public static void Show(string info, GameObject Obj, string function, string yesButtonInfo = "", string noButtonInfo = "", bool showResetPos = false) {
        UIScene_Alert alertView = createAlertView();
        if (!alertView) {
            return;
        }
        alertView.noBtn.gameObject.SetActive(true);
        alertView.yesBtn.gameObject.SetActive(true);
        alertView.label.text = info;

        Vector3 pos = alertView.noBtn.transform.localPosition;
        pos.x = -pos.x;
        alertView.yesBtn.transform.localPosition = pos;

        alertView.SetYesButtonInfo(yesButtonInfo);
        alertView.SetNoButtonInfo(noButtonInfo);

        alertView.SetObj(Obj);
        alertView.SetFunction(function);
        //if (GlobeHelper.currentLevel != null && GlobeHelper.currentLevel.Inst.levelTarget == (int)eLevelTartet.eLT_TeamDrak)
        //{
        //    alertView.obj_ResetPos.SetActive(false);
        //}
        //else
        //{
        //    alertView.obj_ResetPos.SetActive(showResetPos);
        //}
    }

    public static void Show(string info, bool isQuit) {
        UIScene_Alert alertView = createAlertView();
        if (!alertView) {
            return;
        }
        alertView.quitBtn.gameObject.SetActive(true);
        alertView.label.text = info;

        alertView.SetIsQuit(isQuit);
    }

    public static void Show(string info, bool isQuit, GameObject obj, string quitFuntion) {
        UIScene_Alert alertView = createAlertView();
        if (!alertView) {
            return;
        }
        alertView.quitBtn.gameObject.SetActive(true);
        alertView.label.text = info;
        alertView.SetObj(obj);
        alertView.SetFunction(quitFuntion);

        alertView.SetIsQuit(isQuit);
    }

    public static void ShowInput(string info, GameObject Obj, string function, string yesButtonInfo = "", string noButtonInfo = "", string InputPromptInfo = "") {
        UIScene_Alert alertView = createAlertView();
        if (!alertView) {
           return; 
        }
        alertView.noBtn.gameObject.SetActive(true);
        alertView.yesBtn.gameObject.SetActive(true);
        alertView.input.gameObject.SetActive(true);
        alertView.label.text = info;

        Vector3 pos = alertView.noBtn.transform.localPosition;
        pos.x = -pos.x;
        alertView.yesBtn.transform.localPosition = pos;

        alertView.SetYesButtonInfo(yesButtonInfo);
        alertView.SetNoButtonInfo(noButtonInfo);
        alertView.SetInputPromptInfo(InputPromptInfo);

        alertView.SetObj(Obj);
        alertView.SetFunction(function);
    }

    public static void ShowRate(string info) {
        if(Helper.UIAndActive<UIScene_Alert>()){
            return;
        }
        UIScene_Alert alertView = createAlertView();
        if (!alertView)
        {
            return;
        }
        alertView.rateUI.SetActive(true);
        alertView.label.text = info;
    }

    private static int alertIndex;
    private static int startPanelDepth = 11;
    private static UIScene_Alert createAlertView() {
        UIManager uiMgr = Helper.SceneManager<UIManager>();
        GameObject alert = uiMgr.OpenUISceneByName("UIScene_Alert", alertIndex.ToString());
        if (alert) {
            alert.GetComponent<UIPanel>().depth = startPanelDepth + alertIndex;
            AddAlertIndex(1);
            UIScene_Alert alertView = alert.GetComponent<UIScene_Alert>();
            alertView.InitScene();
            return alertView;  
        }

        return null;
    }

    public static void AddAlertIndex(int count)
    {
        alertIndex += count;
    }

    public static void ResetAlertIndex()
    {
        alertIndex = 0;
    }
}
