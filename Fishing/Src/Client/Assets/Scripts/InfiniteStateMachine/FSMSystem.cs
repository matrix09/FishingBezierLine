using System.Collections.Generic;
using AttTypeDefine;
using SimpleJson;
using UnityEngine;

/**
A Finite State Machine System based on Chapter 3.1 of Game Programming Gems 1 by Eric Dybsand
 
Written by Roberto Cezar Bianchini, July 2010
 
 
How to use:
    1. Place the labels for the transitions and the states of the Finite State System
        in the corresponding enums.
 
    2. Write new class(es) inheriting from FSMState and fill each one with pairs (transition-state).
        These pairs represent the state S2 the FSMSystem should be if while being on state S1, a
        transition T is fired and state S1 has a transition from it to S2. Remember this is a Deterministic FSM. 
        You can't have one transition leading to two different states.
 
       Method Reason is used to determine which transition should be fired.
       You can write the code to fire transitions in another place, and leave this method empty if you
       feel it's more appropriate to your project.
 
       Method Act has the code to perform the actions the NPC is supposed do if it's on this state.
       You can write the code for the actions in another place, and leave this method empty if you
       feel it's more appropriate to your project.
 
    3. Create an instance of FSMSystem class and add the states to it.
 
    4. Call Reason and Act (or whichever methods you have for firing transitions and making the NPCs
         behave in your game) from your Update or FixedUpdate methods.
 
    Asynchronous transitions from Unity Engine, like OnTriggerEnter, SendMessage, can also be used, 
    just call the Method PerformTransition from your FSMSystem instance with the correct Transition 
    when the event occurs.
 
 
 
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE 
AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/


/// <summary>
/// FSMSystem class represents the Finite State Machine class.
///  It has a List with the States the NPC has and methods to add,
///  delete a state, and to change the current state the Machine is on.
/// </summary>
public class FSMSystem {
    private readonly List<FSMState> states;
    private FSMState currentState;

    public FSMState CurrentState {
        get {
            return currentState;
        }
    }

    public FSMSystem() {
        states = new List<FSMState>();
    }

    public void AddState(FSMState s) {
        // Check for Null reference before deleting
        if (s == null) {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
            return;
        }

        // First State inserted is also the Initial state,
        //   the state the machine is in when the simulation begins
        if (states.Count == 0) {
            states.Add(s);
            currentState = s;
            currentState.DoBeforeEntering();
            return;
        }

        if (states.Contains(s)) {
            Debug.LogError("FSM ERROR: Impossible to add state " + s.ID.ToString() +
                               " because state has already been added");
            return;
        }

        states.Add(s);
    }

    public void DeleteState(StateID id) {
        // Check for NullState before deleting
        if (id == StateID.NullStateID) {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        FSMState tempS = GetStateByID(id);
        if (tempS != null) {
            states.Remove(tempS);
            return;
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() +
                       ". It was not on the list of states");
    }

    //强制进入一个状态，不判断当前状态是否可转换到新状态
    public void ForceState(StateID id) {
        if (id == StateID.NullStateID) {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        FSMState state = GetStateByID(id);
        if (state == null) {
            Debug.LogError("FSM ERROR: Impossible to force to state " + id.ToString() +
                           ". It was not on the list of states");
            return;
        }

        //退出当前状态
        currentState.DoBeforeLeaving();

        //更换当前状态
        currentState = state;

        //进入新状态
        currentState.DoBeforeEntering();
    }

    //执行状态转换
    public void PerformTransition(StateID id) {
        // Check for NullTransition before changing the current state
        if (id == StateID.NullStateID) {
            Debug.LogError(" FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        if (!currentState.IsHaveTransition(id)) {
            return;
        }

        FSMState state = GetStateByID(id);
        if (state == null) {
            Debug.LogError("FSM ERROR: Impossible to force to state " + id.ToString() +
                           ". It was not on the list of states");
            return;
        }

        //退出当前状态
        currentState.DoBeforeLeaving();

        //更换当前状态
        currentState = state;

        //进入新状态
        currentState.DoBeforeEntering();
    }

    public void ProcessEvent(CombatCondition condition, string param) {
        // Check for NullTransition before changing the current state
        if (condition == CombatCondition.NullCondition) {
            Debug.LogError("FSM ERROR: NullCondition is not allowed for a real condition");
            return;
        }

        if (currentState != null) {
            currentState.DoEvent(condition, param);
        }
    }

    //是否在某一状态内
    public bool IsInState(StateID id) {
        if (id == StateID.NullStateID) {
            return false;
        }

        return CurrentState.ID == id;
    }

    private FSMState GetStateByID(StateID id) {
        int count = states.Count;
        for (int i = 0; i < count; ++i) {
            if (states[i].ID == id) {
                return states[i];
            }
        }

        return null;
    }
}
