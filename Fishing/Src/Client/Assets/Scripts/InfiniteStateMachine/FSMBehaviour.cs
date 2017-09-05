using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Helpers;
using Assets.Scripts.ConsoleController.Console;

public class FSMBehaviour : MonoBehaviour {
    //各个状态用到的参数保存与此

    //public bool forceFallIsMoveZ = true;        //受力落体是否有水平位移
    //public Vector3 forceFallDirectionZ = Vector3.zero;     //受力落体z轴运动方向

    //public Vector3 injuredDownUpDirectionZ = Vector3.zero;  //砸地弹起z轴运动方向

    //public bool isFloatDeath = false;   //是否是浮空死亡

    protected GameObject owner;
    protected FSMSystem fsm;
    //protected FightVideoManager fvmManger;

    // Use this for initialization
    private void Start() {
        MakeFSM();
    }

    // Update is called once per frame
    private void Update() {
        UpdateFSM();
    }

    private void InitData() {
        //forceFallInfo = null;
        //forceFallIsMoveZ = true;
        //forceFallDirectionZ = Vector3.zero;

        //injuredDownUpDirectionZ = Vector3.zero;

        //isFloatDeath = false;
    }

    public bool IsInState(StateID id) {
        if (fsm == null) {
            return false;
        }
        return fsm.IsInState(id);
    }

    public virtual void ForceState(StateID id) {
        if (fsm != null) {
            fsm.ForceState(id);
        }
    }

    public virtual void SetTransition(StateID id) {
        InitData();

        if (fsm != null) {
            this.Log("FSM", "FSMBehavior fsm.PerformTransition " + id);
            fsm.PerformTransition(id);
            //FightVideoManager.Instance().RecordOneFightInfo(,id);
             //Helper.Manager<FightVideoManager>().RecordOneFightInfo((AttTypeDefine.ePlayerObj)gameObject.GetComponent<BaseActor_Player>().powertype, id);
        }
    }

    public void UpdateFSM() {
        if (fsm != null && fsm.CurrentState != null) {
            fsm.CurrentState.Reason();
            fsm.CurrentState.Act();
        }
    }

    public StateID CurrentStateID {
        get {
            if (fsm != null) {
                return fsm.CurrentState.ID;
            }

            return StateID.NullStateID;
        }
    }

    public bool CanUseSkill {
        get {
            if (IsInState(StateID.Free)
             || IsInState(StateID.WaitingNextSkill)) {
                return true;
            }

            return false;
        }
    }

    public bool CanInjuredMove {
        get {
            return IsInState(StateID.Free)
             || IsInState(StateID.InjuredBack)
             || IsInState(StateID.InjuredFloat)
             || IsInState(StateID.InjuredDownUp)
             || IsInState(StateID.ForceFall)
             || IsInState(StateID.Death);
        }
    }

    public void ProcessEvent(CombatCondition condition, string param) {
        if (fsm != null) {
            fsm.ProcessEvent(condition, param);
        }
    }

    private void MakeFSM() {

        BaseActor baseActor = gameObject.GetComponent<BaseActor>();
        if (baseActor == null)
        {
            Debug.LogError("Can not found BaseActor on actor = " + gameObject.name);
            return;
        }

        //FSMState_Birth stateBirth = new FSMState_Birth(baseActor);
        //stateBirth.AddTransition(StateID.Free);
        //stateBirth.AddTransition(StateID.Death);
        //stateBirth.AddTransition(StateID.Run);

        FSM_Free stateFree = new FSM_Free(baseActor);
        stateFree.AddTransition(StateID.CastSkill);
        //stateFree.AddTransition(StateID.SkillForward);
        //stateFree.AddTransition(StateID.OriginBackward);
        //stateFree.AddTransition(StateID.InjuredBack);
        //stateFree.AddTransition(StateID.InjuredFloat);
        //stateFree.AddTransition(StateID.ForceFall);
        //stateFree.AddTransition(StateID.InjuredDown);
        //stateFree.AddTransition(StateID.InjuredDownUp);
        //stateFree.AddTransition(StateID.Death);
        //stateFree.AddTransition(StateID.Run);
        //stateFree.AddTransition(StateID.Freezing);
        //stateFree.AddTransition(StateID.Stone);

        //FSMState_SkillForward stateSF = new FSMState_SkillForward(baseActor);
        //stateSF.AddTransition(StateID.Free);
        //stateSF.AddTransition(StateID.CastSkill);
        //stateSF.AddTransition(StateID.Death);


        FSM_CaseSkill stateCastSkill = new FSM_CaseSkill(baseActor);
        stateCastSkill.AddTransition(StateID.Free);
        //stateCastSkill.AddTransition(StateID.WaitingNextSkill);
        //stateCastSkill.AddTransition(StateID.SkillForward);
        //stateCastSkill.AddTransition(StateID.SkillBackward);
        //stateCastSkill.AddTransition(StateID.Death);

        //FSMState_WaitingNextSkill stateWaitingNextSkill = new FSMState_WaitingNextSkill(baseActor);
        //stateWaitingNextSkill.AddTransition(StateID.Free);
        //stateWaitingNextSkill.AddTransition(StateID.SkillForward);
        //stateWaitingNextSkill.AddTransition(StateID.SkillBackward);
        //stateWaitingNextSkill.AddTransition(StateID.Death);

        //FSMState_SkillBackward stateSKillBackward = new FSMState_SkillBackward(baseActor);
        //stateSKillBackward.AddTransition(StateID.Free);
        //stateSKillBackward.AddTransition(StateID.SkillForward);
        //stateSKillBackward.AddTransition(StateID.Death);

        //FSMState_OriginBackward stateOriginBackward = new FSMState_OriginBackward(baseActor);
        //stateOriginBackward.AddTransition(StateID.Free);
        //stateOriginBackward.AddTransition(StateID.Death);
        //stateOriginBackward.AddTransition(StateID.Freezing);
        //stateOriginBackward.AddTransition(StateID.Stone);

        //FSMState_InjuredBack stateBack = new FSMState_InjuredBack(baseActor);
        //stateBack.AddTransition(StateID.Free);
        //stateBack.AddTransition(StateID.InjuredBack);
        //stateBack.AddTransition(StateID.InjuredFloat);
        //stateBack.AddTransition(StateID.InjuredDown);
        //stateBack.AddTransition(StateID.ForceFall);
        //stateBack.AddTransition(StateID.Death);
        //stateBack.AddTransition(StateID.Freezing);
        //stateBack.AddTransition(StateID.Stone);

        //FSMState_InjuredFloat stateFloat = new FSMState_InjuredFloat(baseActor);
        //stateFloat.AddTransition(StateID.Free);
        //stateFloat.AddTransition(StateID.InjuredBack);
        //stateFloat.AddTransition(StateID.InjuredFloat);
        //stateFloat.AddTransition(StateID.InjuredDown);
        //stateFloat.AddTransition(StateID.ForceFall);
        //stateFloat.AddTransition(StateID.Death);
        //stateFloat.AddTransition(StateID.Freezing);
        //stateFloat.AddTransition(StateID.Stone);

        //FSMState_InjuredDown stateDown = new FSMState_InjuredDown(baseActor);
        //stateDown.AddTransition(StateID.Free);
        //stateDown.AddTransition(StateID.InjuredDownUp);
        //stateDown.AddTransition(StateID.Death);
        //stateDown.AddTransition(StateID.Freezing);
        //stateDown.AddTransition(StateID.Stone);

        //FSMState_InjuredDownUp stateDownUp = new FSMState_InjuredDownUp(baseActor);
        //stateDownUp.AddTransition(StateID.Free);
        //stateDownUp.AddTransition(StateID.InjuredBack);
        //stateDownUp.AddTransition(StateID.InjuredFloat);
        //stateDownUp.AddTransition(StateID.InjuredDown);
        //stateDownUp.AddTransition(StateID.ForceFall);
        //stateDownUp.AddTransition(StateID.Death);
        //stateDownUp.AddTransition(StateID.Freezing);
        //stateDownUp.AddTransition(StateID.Stone);

        //FSMState_ForceFall stateForceFall = new FSMState_ForceFall(baseActor);
        //stateForceFall.AddTransition(StateID.Free);
        //stateForceFall.AddTransition(StateID.InjuredBack);
        //stateForceFall.AddTransition(StateID.InjuredFloat);
        //stateForceFall.AddTransition(StateID.InjuredDown);
        //stateForceFall.AddTransition(StateID.Death);
        //stateForceFall.AddTransition(StateID.Freezing);
        //stateForceFall.AddTransition(StateID.Stone);

        //FSMState_Death stateDeath = new FSMState_Death(baseActor);
        //stateDeath.AddTransition(StateID.Free);
        //stateDeath.AddTransition(StateID.InjuredBack);
        //stateDeath.AddTransition(StateID.InjuredFloat);
        //stateDeath.AddTransition(StateID.ForceFall);
        //stateDeath.AddTransition(StateID.InjuredDown);
        //stateDeath.AddTransition(StateID.InjuredDownUp);
        //stateDeath.AddTransition(StateID.Freezing);
        //stateDeath.AddTransition(StateID.Stone);

        //FSMState_Run stateRun = new FSMState_Run(baseActor);
        //stateRun.AddTransition(StateID.Free);
        //stateRun.AddTransition(StateID.SkillForward);
        //stateRun.AddTransition(StateID.OriginBackward);
        //stateRun.AddTransition(StateID.InjuredBack);
        //stateRun.AddTransition(StateID.InjuredFloat);
        //stateRun.AddTransition(StateID.ForceFall);
        //stateRun.AddTransition(StateID.InjuredDown);
        //stateRun.AddTransition(StateID.InjuredDownUp);
        //stateRun.AddTransition(StateID.Death);
        //stateRun.AddTransition(StateID.Freezing);
        //stateRun.AddTransition(StateID.Stone);

        //FSMState_Freezing stateFreezing = new FSMState_Freezing(baseActor);
        //stateFreezing.AddTransition(StateID.Free);
        //stateFreezing.AddTransition(StateID.Death);
        //stateFreezing.AddTransition(StateID.Stone);

        //FSMState_Stone stateStone = new FSMState_Stone(baseActor);
        //stateStone.AddTransition(StateID.Free);
        //stateStone.AddTransition(StateID.Death);
        //stateStone.AddTransition(StateID.Freezing);


        fsm = new FSMSystem();
        //fsm.AddState(stateBirth);
        fsm.AddState(stateFree);
        //fsm.AddState(stateSF);
        fsm.AddState(stateCastSkill);
        //fsm.AddState(stateWaitingNextSkill);
        //fsm.AddState(stateSKillBackward);
        //fsm.AddState(stateOriginBackward);
        //fsm.AddState(stateBack);
        //fsm.AddState(stateFloat);
        //fsm.AddState(stateDown);
        //fsm.AddState(stateDownUp);
        //fsm.AddState(stateForceFall);
        //fsm.AddState(stateDeath);
        //fsm.AddState(stateRun);
        //fsm.AddState(stateFreezing);
        //fsm.AddState(stateStone);
    }

    public bool CanMove() {
        if (fsm == null) {
            return true;
        }

        if (CurrentStateID == StateID.Free) {
            return true;
        }

        return false;
    }
}