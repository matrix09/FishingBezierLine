using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Assets.Scripts.Utilities;
using Mono.Data.Sqlite;
using UnityEngine;
using Assets.Scripts.DataStore;

public class CheckDataStoreUpdate : MonoBehaviour {

    //1:拷贝配置文件（非iOS） 2:拷贝配置文件（iOS）
    //3:拷贝本地数据库（非iOS） 4:拷贝本地数据库（iOS） 5:检测是否有更新 6:下载更新数据库
    public int state = 0;
    private byte[] bytes = null;
    public WWW www = null;
    public string resultLog = "";
    private string dbFileName = "";
    private string verFileName = "";
    private byte[] verDownloadBytes = null;
    public int finishState = 0;   //0：未完成 1:完成正常 2:完成报错

    private bool isCheckUpdate = false;

    // Use this for initialization
    void Start() {
        SystemDataStore.Instance.Disconnect();
        //StartCheck("GameDataStore.db","DBConfig.ini");
    }

    // Update is called once per frame
    void Update() {
        if (state == 1) {
            if (www != null && www.isDone) {
                state = 0;
                CopyConfigFileFinished(www.bytes);
            }
        }
        else if (state == 2) {
            state = 0;
            CopyConfigFileFinished(bytes);
        }
        else if (state == 3) {
            if (www != null && www.isDone) {
                state = 0;
                CopyDBFileFinished(www.bytes);
            }
        }
        else if (state == 4) {
            state = 0;
            CopyDBFileFinished(bytes);
        }
        else if (state == 5) {
            if (www != null && www.isDone) {
                if (www.error != null) {
                    state = 0;
                    finishState = 2;
                    resultLog = "无法链接到更新服务器，请检查网络连接是否正常";
                }
                else {
                    state = 0;
                    CheckDBUpdateFinished(www.text, www.bytes);
                }

            }
        }
        else if (state == 6) {
            if (www != null && www.isDone) {
                if (www.error != null) {
                    state = 0;
                    finishState = 2;
                    resultLog = "无法链接到更新服务器，请检查网络连接是否正常";
                }
                else {
                    state = 0;
                    UpdateDBFinished(www.bytes);
                }

            }
        }

    }

    public void StartCheck(string dbName, string verName) {
        dbFileName = dbName;
        verFileName = verName;

        resultLog = "开始检测是否需要拷贝数据库";
        if (CheckNeedCopyDBFile()) {
            CopyDBFileBegin();
        }
        else {
            CheckDBUpdateBegin();
        }
    }

    private void CopyConfigFileBegin() {
        resultLog = "开始拷贝配置文件";
        state = 1;
        GetSourceData(verFileName);
    }

    private void CopyConfigFileFinished(byte[] inBytes) {
        TryCopyFile(inBytes, verFileName);
        CheckDBUpdateBegin();
    }

    private void CopyDBFileBegin() {
        resultLog = "开始拷贝数据库文件";
        state = 3;
        GetSourceData(dbFileName);
    }

    private void CopyDBFileFinished(byte[] inBytes) {
        TryCopyFile(inBytes, dbFileName);
        CopyConfigFileBegin();
    }

    //检测是否有数据库更新
    private void CheckDBUpdateBegin() {
#if UNITY_EDITOR
        if (!isCheckUpdate) {
            finishState = 1;
            return;
        }
#endif
        resultLog = "开始检测是否有数据库更新";
        state = 5;
        string versionURL = "http://1251044271.cdn.myqcloud.com/1251044271/BONS/DataStore/DBConfig.ini?" + Guid.NewGuid();
        www = new WWW(versionURL);
        Download(www);
    }

    private void CheckDBUpdateFinished(string inContent, byte[] inBytes) {
        Debug.Log("服务器数据库版本号为：" + inContent);
        int sourceVersion = GetVersionNum(inContent);

        string verPath = Application.persistentDataPath + "/" + verFileName;
        int targetVersion = 0;
        try {
            // copy database to real file into cache folder
            using (StreamReader reader = new StreamReader(verPath, Encoding.Default)) {
                string content = reader.ReadToEnd();
                Debug.Log("本地数据库版本号为：" + content);
                targetVersion = GetVersionNum(content);
                reader.Close();
            }
        }
        catch (Exception e) {
            Debug.Log("\nTest Fail with Exception " + e);
            Debug.Log("\n\n Did you copy test.db into StreamingAssets ?\n");
        }

        if (sourceVersion > targetVersion) {
            //记录配置文件数据
            verDownloadBytes = inBytes;

            //开始更新数据库
            UpdateDBBegin();
        }
        else {
            //todo
            resultLog = "数据已经是最新的了";
            finishState = 1;
        }
    }

    private void UpdateDBBegin() {
        resultLog = "开始更新数据库文件";
        state = 6;
        string dbURL = "http://1251044271.cdn.myqcloud.com/1251044271/BONS/DataStore/GameDataStore.db?" + Guid.NewGuid();
        www = new WWW(dbURL);
        Download(www);
    }

    private void UpdateDBFinished(byte[] inBytes) {
        //替换数据库文件
        TryCopyFile(inBytes, dbFileName);

        //替换配置文件
        TryCopyFile(verDownloadBytes, verFileName);

        resultLog = "更新完成";
        finishState = 1;
    }

    public bool CheckNeedCopyDBFile() {
        string filename = Application.persistentDataPath + "/" + dbFileName;
        if (!File.Exists(filename)) {
            return true;
        }

        string versionName = Application.persistentDataPath + "/" + verFileName;
        if (!File.Exists(versionName)) {
            return true;
        }

        string sourceCotent = "";
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        string vertionPath = "file://" + Application.streamingAssetsPath + "/" + verFileName;
        www = new WWW(vertionPath);
        Download(www);
        while (!www.isDone) {
        }
        sourceCotent = www.text;
#elif UNITY_WEBPLAYER
            string vertionPath = "StreamingAssets/" + verFileName;
            www = new WWW(vertionPath);
            Download(www);
            while (!www.isDone) {
            }
            sourceCotent = www.text;
#elif UNITY_IPHONE
            string vertionPath = Application.dataPath + "/Raw/" + verFileName;
            try{	
                using ( StreamReader reader = new StreamReader(vertionPath, Encoding.Default) ){
                    sourceCotent = reader.ReadToEnd();
                    reader.Close();
                }			
            } catch (Exception e){
            //log += 	"\nTest Fail with Exception " + e.ToString();
            //log += 	"\n";
            }
#elif UNITY_ANDROID
            string vertionPath = Application.streamingAssetsPath + "/" + verFileName;
            www = new WWW(vertionPath);
            Download(www);
            while (!www.isDone) {
            }
            sourceCotent = www.text;
#endif

        string targetContent = "";
        try {
            // copy database to real file into cache folder
            using (StreamReader reader = new StreamReader(versionName, Encoding.Default)) {
                targetContent = reader.ReadToEnd();
                reader.Close();
            }
        }
        catch (Exception e) {
            Debug.Log("\nTest Fail with Exception " + e);
            Debug.Log("\n\n Did you copy test.db into StreamingAssets ?\n");
        }

        if (GetVersionNum(sourceCotent) > GetVersionNum(targetContent)) {
            return true;
        }

        return false;
    }

    private int GetVersionNum(string verContent) {
        int version = 0;
        string[] strs = verContent.Split('=');
        if (strs.Length > 1) {
            version = int.Parse(strs[1].Trim());
        }
        return version;
    }

    //获取 StreamingAssets 下数据
    private void GetSourceData(string fileName) {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        string filePath = "file://" + Application.streamingAssetsPath + "/" + fileName;
        www = new WWW(filePath);
        Download(www);
#elif UNITY_WEBPLAYER
            string filePath = "StreamingAssets/" + fileName;
            www = new WWW(filePath);
            Download(www);
#elif UNITY_IPHONE
            state += 1;
            string filePath = Application.dataPath + "/Raw/" + fileName;
            try{	
                using ( FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read) ){
                    bytes = new byte[fs.Length];
                    fs.Read(bytes,0,(int)fs.Length);
                    fs.Close();
                }			
            } catch (Exception e){
            //log += 	"\nTest Fail with Exception " + e.ToString();
            //log += 	"\n";
            }
#elif UNITY_ANDROID
            string filePath = Application.streamingAssetsPath + "/" + fileName;
            www = new WWW(filePath);
            Download(www);
#endif
    }

    //拷贝文件到使用目录
    private void TryCopyFile(byte[] bytes, string fileName) {
        if (bytes == null) {
            return;
        }

        string filePath = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(filePath)) {
            File.Delete(filePath);
        }

        if (bytes != null) {
            try {
                // copy database to real file into cache folder
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                    Debug.Log("Copy database from streaminAssets to persistentDataPath: " + filePath);
                }
            }
            catch (Exception e) {
                Debug.Log("\nTest Fail with Exception " + e);
                Debug.Log("\n\n Did you copy test.db into StreamingAssets ?\n");
            }
        }
    }

    private IEnumerator ProcessDownload(WWW www) {
        yield return www;
    }

    private void Download(WWW www) {
        StartCoroutine(ProcessDownload(www));
        //         while (!www.isDone) {
        //             Debug.Log("=====" + www.progress);
        //         }
    }
}
