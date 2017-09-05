using UnityEngine;
using System.Collections;

public class FSM_CaseSkill : FSMState
{

    public FSM_CaseSkill(BaseActor ba)
    {
        owner = ba;
        stateID = StateID.CastSkill;

    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();

        owner.AnimMgr.StartAnimation(owner.SkillMgr.CurrentSkill.AnimName, SkillEvent, null, null, null, AnimClip_End);
    }

    private void SkillEvent (CombatCondition cc)
    {
        owner.FSMB.ProcessEvent(cc, "WaitingForNextSkill");
    }

    private void AnimClip_End ()
    {
        owner.FSMB.SetTransition(StateID.Free);
            
    }

    public override void Reason()
    {

    }

    public override void Act ()
    {

    }

    public override void DoEvent(CombatCondition condition, string param)
    {
        base.DoEvent(condition, param);

        if(param == "WaitingForNextSkill")
        {
            if (condition == CombatCondition.waitingNextSKill)
            {
                owner.FSMB.SetTransition(StateID.Free);
            }
        }
       

    }


}
