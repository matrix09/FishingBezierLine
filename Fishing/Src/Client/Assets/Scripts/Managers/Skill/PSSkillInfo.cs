using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers.DisablePool;
namespace Assets.Scripts.Managers.Skill
{

    public class PSSkillInfo : MonoBehaviour
    {
        public float SkillDuration = 0f;

        public bool IgnoreTimeScale = false;

        

        private float fStartTime = 0f;

        // Use this for initialization
        void Start()
        {

            fStartTime = GlobeHelper.GetCurrentTime(IgnoreTimeScale);
        }

        // Update is called once per frame
        void Update()
        {
            if (GlobeHelper.GetCurrentTime(IgnoreTimeScale) - fStartTime >= SkillDuration)
            {
                DestroySkill();
            }
        }

        //销毁技能prefab
        public void DestroySkill()
        {
            DisabledPool.AddToDisablePool(gameObject.name);

            Debug.Log("PSSkillInfo : DestroySkill");
        }
    }
}

