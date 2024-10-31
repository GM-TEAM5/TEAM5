using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public FSMState currState;

    private void Update()
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
        
        currState = newState;
        currState.OnEnter();
    }


}
