using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers.Attributes;
namespace Assets.Scripts.Managers.Skill
{
    public class SkillManager : MonoBehaviour
    {
        private BaseActor owner;
        public BaseActor Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        private SkillBase currentskill;
        public SkillBase CurrentSkill
        {
            get
            {
                return currentskill;
            }
            set
            {
                if (value != currentskill)
                    currentskill = value;
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UseSkill (int skillid)
        {
            currentskill = DataRecordManager.TempBase<SkillBase>(skillid);

            Owner.FSMB.SetTransition(StateID.CastSkill);

        }





    }

}

