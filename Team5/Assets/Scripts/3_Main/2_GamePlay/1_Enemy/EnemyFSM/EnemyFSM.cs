using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyState
{
    STOPMOVE,
    APPROACH,
    RETREAT,
    ATTACK
}


public class EnemyFSM : FSM
{
    Enemy enemy;
    NavMeshAgent navAgent;

    Dictionary<EnemyState, EnemyFSMState> states;

    //============================================================================

    public void Init(Enemy enemy, NavMeshAgent navAgent)
    {
        this.enemy = enemy;
        this.navAgent = navAgent;
        
        states = new()
        {
            {EnemyState.STOPMOVE, new  EnemyState_StopMove(enemy,navAgent) },
            {EnemyState.APPROACH, new EnemyState_Approach(enemy,navAgent) },
            {EnemyState.RETREAT, new  EnemyState_Retreat(enemy,navAgent) },
            {EnemyState.ATTACK, new  EnemyState_Attack(enemy,navAgent) },


        };
    }

    //============================================================================

    public void SetState_StopMove()
    {
        ChangeState( states[EnemyState.STOPMOVE] );
    }

    public void SetState_Approach()
    {
        ChangeState( states[EnemyState.APPROACH] );
    }

    public void SetState_Retreat()
    {
        ChangeState( states[EnemyState.RETREAT] );
    }

    public void SetState_Attack()
    {
        ChangeState( states[EnemyState.ATTACK] );
    }

    //
}
