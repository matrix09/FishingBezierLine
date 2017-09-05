using Assets.Scripts.DataStore;
using System;
using System.Collections.Generic;
using Assets.Scripts.Managers.Attributes;
public static class DataRecordManager {

    private static readonly Dictionary<Type, Dictionary<int, object>> datas = new Dictionary<Type, Dictionary<int, object>>();

    //获取 class instance
    public static T TempBase<T>(int id) where T : BaseClass {
        Type _type = typeof(T);
        T _inst;
        if (!datas.ContainsKey(_type)) {
            datas.Add(_type, new Dictionary<int, object>());
        }  

        if (!datas[_type].ContainsKey(id)) {
            _inst = (T)System.Activator.CreateInstance(typeof(T), id);
            datas[_type].Add(id, _inst);
        }
        else {
            _inst = (T)datas[_type][id];

        }
        return _inst;
    }

    //获取Record
    public static SystemDataStore.DataRecord GetDataRecord<T>(int id, out bool isValide) {

        SystemDataStore.DataRecord record = SystemDataStore.Instance.FetchRecord<T>(id);
        isValide = false;
        if (null != record) {
            if (record.Read()) {
                isValide = true;
                return record;
            }
        }

        return null;
    }

    //清理数据
    public static void ClearDataRecordStoreManager() {
        datas.Clear();
    }

}
