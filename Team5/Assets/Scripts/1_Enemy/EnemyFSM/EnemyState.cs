using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public abstract class EnemyFSMState : FSMState
{
    public Enemy enemy;
    public NavMeshAgent navAgent;

    public EnemyFSMState(Enemy enemy, NavMeshAgent navAgent)
    {
        this.enemy = enemy;
        this.navAgent = navAgent;
    }
}



/// <summary>
/// 가만히 서있을 때,
/// </summary>
public class EnemyState_StopMove : EnemyFSMState
{
    public EnemyState_StopMove(Enemy enemy, NavMeshAgent navAgent) : base(enemy, navAgent)
    {

    }

    public override void OnEnter()
    {
        navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        
        
    }




}


/// <summary>
/// 플레이어 쪽으로 접근할 때, 
/// </summary>
public class EnemyState_Approach : EnemyFSMState
{
    public EnemyState_Approach(Enemy enemy, NavMeshAgent navAgent) : base(enemy, navAgent)
    {
    }

    public override void OnEnter()
    {
        navAgent.isStopped = false;
        navAgent.velocity = navAgent.desiredVelocity; 
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        navAgent.SetDestination(enemy.t_target.position);   
    }
}

/// <summary>
/// 플레이어로부터 멀어질때, 
/// </summary>
public class EnemyState_Retreat : EnemyFSMState
{
    Vector3 currDest;
    float destDistSqr;
    
    public EnemyState_Retreat(Enemy enemy, NavMeshAgent navAgent) : base(enemy, navAgent)
    {
    }

    public override void OnEnter()
    {
        navAgent.isStopped = false;
        navAgent.velocity = navAgent.desiredVelocity; 
        currDest = enemy.t.position;
        
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        destDistSqr = (currDest - enemy.t.position).sqrMagnitude;
        // Debug.Log($"{destDistSqr} = {currDest} ");
        if (destDistSqr < 2f)
        {
            SetNewDest();
        }
        // 
        navAgent.stoppingDistance = 0;
        navAgent.SetDestination(currDest );
    }


    void SetNewDest()
    {
        Vector3 targetPos = enemy.t_target.position;
        currDest = enemy.t.position + (enemy.t.position - targetPos).normalized * 2f;
        

        // 첫번째 dest 가 navMeshSurface 위의 좌표가 아니라면, 플레이어 근처의 랜덤 좌표를 dest 로 지정한다. 
        if (NavMesh.SamplePosition(currDest , out NavMeshHit hit, 0.5f, NavMesh.AllAreas) == false)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = targetPos + Random.insideUnitSphere * 5f;
                NavMeshHit hit2;
                if (NavMesh.SamplePosition(randomPoint, out hit2, 0.5f, NavMesh.AllAreas))
                {
                    currDest  = hit2.position;

                    break;
                }
            }
        }

    }
}

/// <summary>
/// 기술을 쓸 때, - 
/// </summary>
public class EnemyState_Attack : EnemyFSMState
{
    public EnemyState_Attack(Enemy enemy, NavMeshAgent navAgent) : base(enemy, navAgent)
    {

    }

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        
    }
}


/// <summary>
/// 기술을 쓸 때, - 
/// </summary>
public class EnemyState_DirectMove : EnemyFSMState
{
    public EnemyState_DirectMove(Enemy enemy, NavMeshAgent navAgent) : base(enemy, navAgent)
    {

    }

    public override void OnEnter()
    {
        navAgent.isStopped = false;
        navAgent.velocity = navAgent.desiredVelocity; 
    
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        
    }
}
