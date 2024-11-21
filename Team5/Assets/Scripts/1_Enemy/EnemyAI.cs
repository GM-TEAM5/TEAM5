using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


using BW.Util;



[RequireComponent(typeof(NavMeshAgent),
                    typeof(EnemyFSM))]

public class EnemyAI : MonoBehaviour
{
    // Transform t;            // 본체 트랜스폼
    // Transform t_target;     // 타겟의 트랜스폼
    // public float targetDistSqr;


    public EnemyAbilitySystem abilitySystem;
    public EnemyFSM fsm;
    public NavMeshAgent navAgent;
    
    Enemy enemy;

    public float playerDetectionRange = 10;
    public float abilityRange;
    public float retreatRange;
    public float reationRate = 0.3f;

    public float lastReactionTime;

    public bool canUpdate => Time.time >= lastReactionTime + reationRate;


    


    //=================================
    void Update()
    {
        if (GamePlayManager.isGamePlaying == false)
        {
            OnStopped();
        }
    }


    public bool TryUpdate()
    {        
        if (canUpdate == false || abilitySystem.isCasting || enemy.stunned )
        {
            return false;
        }
        lastReactionTime = Time.time;
        
        
        //
        abilitySystem.SetCurrAbility();
        //
        abilityRange = abilitySystem.currAbilityRange;
        navAgent.stoppingDistance = abilityRange;
        
        
        // 0. 사용할 기술을 정하고,
        // 1. 타겟을 정하고,
        // 2. 
        enemy.targetDistSqr =  (enemy.t_target.position - enemy.t.position).sqrMagnitude;
        
        //
        if ( enemy.targetDistSqr <= abilityRange * abilityRange)   // 공격사거리 안 일때,
        {
            abilitySystem.TryUse();
            
            //
            if (retreatRange>0 && enemy.targetDistSqr <= retreatRange * retreatRange)   // 후퇴거리 안 일때, 
            {
                fsm.SetState_Retreat();
            }
            else
            {
                fsm.SetState_StopMove();
            }
        }
        else    // 거리 밖일 때, 
        {
            
            OnTargetOutofAbilityRange();
        }

        //
        fsm.UpdateFSM();        // fsm 업뎃
        return true;
    }



    public void Init(Enemy enemy)
    {
        this.enemy = enemy;

        navAgent = GetComponent<NavMeshAgent>();
        navAgent.isStopped = false;
        navAgent.autoBraking = false;
        navAgent.speed = enemy.data.movementSpeed;

        fsm = GetComponent<EnemyFSM>();
        fsm.Init(enemy,navAgent);

        abilitySystem = GetComponent<EnemyAbilitySystem>();
        abilitySystem.Init(enemy);


        retreatRange = enemy.data.retreatRange;
    }




    void OnTargetInAbilityRange( )
    {
        
    }


    void OnTargetOutofAbilityRange()
    {
        if (enemy.stopped)
        {
            return;
        }

        fsm.SetState_Approach();
    }

    void Retreat()
    {
        
        fsm.SetState_Retreat();
    }

    //================================

    public void OnStunned()
    {
        abilitySystem.Interrupt(enemy);
        fsm.SetState_StopMove();
    }



    public void OnStopped()
    {
        fsm.SetState_StopMove();
    }

    public void OnDie()
    {
        fsm.SetState_StopMove();
    }

    //==================================
}
