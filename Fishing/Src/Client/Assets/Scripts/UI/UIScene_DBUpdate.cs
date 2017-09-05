//author : zhaoxuefei
//date : 2015/12/21
using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using Assets.Scripts.Utilities;

public class UIScene_DBUpdate : UIScene {

    public GameObject sliderParent;
    public GameObject skipButton;
    public UISlider slider_probar;
    public UILabel label_probar;
    public UILabel lblStatus;

    public CheckDataStoreUpdate updateIns;

    private void Start() {
        slider_probar.value = 0;
        label_probar.text = "0%";
        ShowSlider(false);
        skipButton.SetActive(false);
        updateIns.StartCheck("GameDataStore.db", "DBConfig.ini");
    }

    private void Update() {
        lblStatus.text = updateIns.resultLog;
        if (updateIns.state == 6) {
            ShowSlider(true);
            slider_probar.sliderValue = updateIns.www.progress;
            label_probar.text = GetPercentString(updateIns.www.progress);
        }
        else {
            ShowSlider(false);
        }

        if (updateIns.finishState == 1) {
            if (!IsInvoking("DelayOpenLoginUI")) {
                Invoke("DelayOpenLoginUI", 0.5f);
            }
        }
        else if (updateIns.finishState == 2) {
            skipButton.SetActive(true);
        }
    }

    private void DelayOpenLoginUI() {
        Helper.Manager<UIManager>().OpenUISceneByName("UIScene_Login");
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void ShowSlider(bool bShow) {
        if (bShow) {
            sliderParent.SetActive(true);
            slider_probar.value = 0f;
        }
        else {
            sliderParent.SetActive(false);
        }
    }

    private string GetPercentString(float percent) {
        int percentInt = (int)(percent * 10000);
        int dotPercent = percentInt % 100;
        string dotPercentStr = "";
        if (dotPercent == 0) {
            dotPercentStr = "00";
        }
        else if (dotPercent < 10) {
            dotPercentStr = dotPercent + "0";
        }
        else {
            dotPercentStr = dotPercent.ToString();
        }
        return percentInt / 100 + "." + dotPercentStr + "%";
    }

    private void OnPressSkip(GameObject obj) {
        DelayOpenLoginUI();
    }
}




