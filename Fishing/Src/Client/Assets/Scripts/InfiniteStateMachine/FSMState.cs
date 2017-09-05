using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;

public enum StateID {
    NullStateID = 0, // Use this ID to represent a non-existing State in your system	
    Birth,              //出生
    Free,               //待机
    SkillForward,       //技能前冲 
    CastSkill,          //施放技能
    WaitingNextSkill,   //等待下一段技能
    SkillBackward,      //技能后撤     
    OriginBackward,     //原点后撤
    InjuredBack,        //受伤位移-水平
    InjuredFloat,       //受伤位移-浮空
    InjuredDown,        //受伤位移-砸地下落
    InjuredDownUp,      //受伤位移-砸地弹起
    ForceFall,          //受力下落
    Death,              //死亡
    Run,                //行走
    Freezing,           //冰冻
    Stone,              //石化
}

public enum CombatCondition {
    NullCondition = 0,
    waitingNextSKill,       //等待下一段技能输入
    SkillAnimEnd,           //技能动作结束
    Injured,                //挨揍
    AirBlockFrontBack,      //前后空气墙碰撞
    AirBlockTop,            //顶部空气墙碰撞
}

public abstract class FSMState {
    protected List<StateID> outputStates = new List<StateID>();
    protected StateID stateID;
    protected BaseActor owner;

    public StateID ID {
        get {
            return stateID;
        }
    }

    public void AddTransition(StateID trans) {
        // Check if anyone of the args is invalid
        if (trans == StateID.NullStateID) {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        // Since this is a Deterministic FSM,
        //   check if the current transition was already inside the map
        if (IsHaveTransition(trans)) {
            Debug.LogError("FSMState ERROR: State " + stateID.ToString() + " already has transition " + trans.ToString() +
                           "Impossible to assign to another state");
            return;
        }

        outputStates.Add(trans);
    }

    /// <summary>
    /// This method deletes a pair transition-state from this state's map.
    /// If the transition was not inside the state's map, an ERROR message is printed.
    /// </summary>
    public void DeleteTransition(StateID trans) {
        // Check for NullTransition
        if (trans == StateID.NullStateID) {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed");
            return;
        }

        // Check if the pair is inside the map before deleting
        if (IsHaveTransition(trans)) {
            outputStates.Remove(trans);
            return;
        }

        Debug.LogError("FSMState ERROR: Transition " + trans.ToString() + " passed to " + stateID.ToString() +
                       " was not on the state's transition list");
    }



    /// <summary>
    /// This method is used to set up the State condition before entering it.
    /// It is called automatically by the FSMSystem class before assigning it
    /// to the current state.
    /// </summary>
    public virtual void DoBeforeEntering() {

    }

    /// <summary>
    /// This method is used to make anything necessary, as reseting variables
    /// before the FSMSystem changes to another one. It is called automatically
    /// by the FSMSystem before changing to a new state.
    /// </summary>
    public virtual void DoBeforeLeaving() {

    }

    /// <summary>
    /// This method decides if the state should transition to another on its list
    /// NPC is a reference to the object that is controlled by this class
    /// </summary>
    //     public virtual void Update() {
    // 
    //     }

    /// <summary>
    /// This method decides if the state should transition to another on its list
    /// NPC is a reference to the object that is controlled by this class
    /// </summary>
    public abstract void Reason();

    /// <summary>
    /// This method controls the behavior of the NPC in the game World.
    /// Every action, movement or communication the NPC does should be placed here
    /// NPC is a reference to the object that is controlled by this class
    /// </summary>
    public abstract void Act();

    public virtual void DoEvent(CombatCondition condition, string param) {
    }

    //判断是否可执行某一转换
    public bool IsHaveTransition(StateID trans) {
        return outputStates.Contains(trans);
    }

    public void CorrectOffsetZ() {
        //if (owner && !GlobeHelper.IsTestScene()) {
        //    Vector3 pos = owner.ATransform.position;
        //    //pos.z = owner.vOrigFightPos.z;
        //    //owner.ATransform.position = pos;
        //}
    }
}
