using UnityEngine;
using System.Collections;
using System;

public class FSM_Free : FSMState {

    public FSM_Free(BaseActor ba)
    {
        owner = ba;
        stateID = StateID.Free;
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();

        owner.AnimMgr.StartAnimation("Base Layer.Idle");
    }

    public override void Reason()
    {
      
    }

    public override void Act()
    {
       
    }


}
