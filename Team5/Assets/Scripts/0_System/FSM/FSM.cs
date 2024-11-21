using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public FSMState currState;

    public void UpdateFSM()
    {
        if (currState != null)
        {
            currState.OnUpdate();
        }
    }

    
 // 상태 전환 메서드
    public void ChangeState(FSMState newState)
    {
        if (currState != null)
        {
            
            currState.OnExit();
        }
        
        if (currState != newState)
        {
            currState = newState;
            currState.OnEnter();        
        }
        
        
    }


}
