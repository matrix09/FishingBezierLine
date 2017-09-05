using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStore;
using System;
using Assets.Scripts.Managers.Skill;

namespace Assets.Scripts.Managers.Attributes
{
    public class SkillBase : BaseClass
    {
        public int AttId;
        public string AnimName;
        public string SkillResourcePath; 
        private bool isValid = false;
		
        [NonSerialized()]
        private PSSkillInfo psskillinfo;
		
        public SkillBase (int id)
        {
            var record = DataRecordManager.GetDataRecord<SkillTemplate>(id, out isValid);
            if (null != record && isValid)
            {
                AttId = record.GetInt(SkillTemplate.AttId);

                AnimName = record.GetString(SkillTemplate.AnimName);

                SkillResourcePath = record.GetString(SkillTemplate.SkillResourcePath);
            }
        } 

        public PSSkillInfo PSSkillInfo
        {
            get
            {
                if(psskillinfo == null && isValid)
                {
                    GameObject obj = Resources.Load(SkillResourcePath) as GameObject;
                    if(null != obj)
                    {
                        psskillinfo = obj.GetComponent<PSSkillInfo>();
                    }
                }

                return psskillinfo;

            }
        }

    }
}

