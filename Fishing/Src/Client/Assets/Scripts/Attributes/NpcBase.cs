using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStore;
namespace Assets.Scripts.Managers.Attributes
{
    public class NpcBase : BaseClass
    {
        public int AttId;
        public int RoleId;
        public int GenderType;
        public string RoleName;
        public string ModelPath;
        public bool isValid;
        public NpcBase(int id)
        {
            var record = DataRecordManager.GetDataRecord<InitNpcTemplate>(id, out isValid);
            if (null != record && isValid)
            {
                AttId = record.GetInt(InitNpcTemplate.AttId);
                RoleId = record.GetInt(InitNpcTemplate.RoleId);
                GenderType = record.GetInt(InitNpcTemplate.GenderType);
                ModelPath = record.GetString(InitNpcTemplate.ModelPath);
                RoleName = GlobeHelper.LocaTempToStr(RoleId);
            }
        }
    }
}
