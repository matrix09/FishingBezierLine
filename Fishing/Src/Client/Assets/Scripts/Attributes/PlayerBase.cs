using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStore;
namespace Assets.Scripts.Managers.Attributes
{
    public class PlayerBase : BaseClass
    {
        public int AttId;
        public int RoleId;
        public int GenderType;
        public string RoleName;
        public string ModelPath;
        public bool isValid;
        public PlayerBase(int id)
        {
            var record = DataRecordManager.GetDataRecord<InitMajorTemplate>(id, out isValid);
            if (null != record && isValid)
            {
                AttId = record.GetInt(InitMajorTemplate.AttId);
                RoleId = record.GetInt(InitMajorTemplate.RoleId);
                GenderType = record.GetInt(InitMajorTemplate.GenderType);
                ModelPath = record.GetString(InitMajorTemplate.ModelPath);
                RoleName = GlobeHelper.LocaTempToStr(RoleId);
            }
        }
    }
}
