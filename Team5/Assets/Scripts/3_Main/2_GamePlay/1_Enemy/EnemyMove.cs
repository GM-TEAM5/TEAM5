using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// 적들의 움직임 AI - 추후 비해비어 트리로 변경할거임.
/// </summary>
public class EnemyMove : MonoBehaviour
{
    Enemy enemy;

    Transform t;

    Coroutine moveRoutine ;

    Vector3 rangeOffset;

    // public void StartMoveRoutine(Enemy enemy)
    // {
       
    //     if (moveRoutine!= null)
    //     {
    //         StopCoroutine(moveRoutine);
    //     }
    //     moveRoutine = StartCoroutine(MoveRoutine());
    // }

    // IEnumerator MoveRoutine()
    // {
    //     var moveCooltime = new WaitForSeconds( enemy.enemyData.moveCooltime );
    //     var waitWhileStunned = new WaitWhile(()=>enemy.stunned);

    //     while(GamePlayManager.isGamePlaying)
    //     {
    //         if (enemy.isAlive )
    //         {
    //             // yield return waitWhileStunned;
                
    //             Move( enemy.enemyData, enemy.navAgent, enemy.t_target.position );
    //         }
    //         else
    //         {
    //             yield break;            
    //         }
    //         yield return moveCooltime;            
    //     }
    // }

    public void Init(Enemy enemy)
    {
        t = transform;
        this.enemy = enemy;
    }

    /// <summary>
    /// 타입에 따라 움직임 - 
    /// </summary>
    /// <param name="navAgent"></param>
    /// <param name="targetPos"></param>
    public void Move(EnemyDataSO enemyData, NavMeshAgent navAgent, Vector3 targetPos)
    {        
        switch (enemyData.attackType)
        {
            case EnemyAttackType.Melee:
                SimpleApproach(navAgent,targetPos);
                break;
            case EnemyAttackType.Range:
                Kite(navAgent,targetPos);
                break;
        }
    }

    /// <summary>
    ///  타겟 위치로 이동 - 사거리 끝까지 이동한다. 
    /// </summary>
    /// <param name="navAgent"></param>
    /// <param name="targetPos"></param>
    void SimpleApproach( NavMeshAgent navAgent, Vector3 targetPos)
    {
        // Vector3 dir = (t.position - targetPos).normalized;
        // Vector3 dest = targetPos + dir * enemy.range;


        navAgent.SetDestination(targetPos);
    }

    /// <summary>
    ///  타겟 위치로 이동 - 사거리 끝까지 이동한다. 
    /// </summary>
    /// <param name="navAgent"></param>
    /// <param name="targetPos"></param>
    void Approach( NavMeshAgent navAgent, Vector3 targetPos)
    {
        Vector3 dir = (t.position - targetPos).normalized;
        Vector3 dest = targetPos + dir * enemy.range;


        navAgent.SetDestination(dest);
    }


    /// <summary>
    /// 카이팅함 - 타겟이 사거리 밖이면 접근하고, 안이면 사거리에 걸치도록 도망감.
    /// </summary>
    void Kite(NavMeshAgent navAgent, Vector3 targetPos)
    {
        float distSqr = Vector3.SqrMagnitude(targetPos - t.position);
        
        
        float rangeSqr = enemy.range * enemy.range;
        //  Debug.Log( $"-- {distSqr}  {rangeSqr}" );
        if(distSqr < rangeSqr)
        {
            if ( distSqr < rangeSqr * 0.7f) // 가중치 부여  - 안그러면 튕기는 움직임이 됨.
            {
                // Debug.Log("도망");
                RunAway(navAgent,targetPos);
            }
            else
            {
                
            }
        }
        else
        {
            // Debug.Log("접근");
            Approach(navAgent,targetPos);
        }

    }

    /// <summary>
    /// 타겟으로 부터 도망.
    /// </summary>
    /// <param name="navAgent"></param>
    /// <param name="targetPos"></param>
    void RunAway(NavMeshAgent navAgent, Vector3 targetPos)
    {
        Vector3 dest = t.position + (t.position - targetPos).normalized * 2f;
        
        // 첫번째 dest 가 navMeshSurface 위의 좌표가 아니라면, 플레이어 근처의 랜덤 좌표를 dest 로 지정한다. 
        if (NavMesh.SamplePosition(dest, out NavMeshHit hit, 1.0f, NavMesh.AllAreas) == false)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = targetPos + Random.insideUnitSphere * 5f;
                NavMeshHit hit2;
                if (NavMesh.SamplePosition(randomPoint, out hit2, 1.0f, NavMesh.AllAreas))
                {
                    dest = hit2.position;
                    break;
                }
            }
        }
  
        // 
        navAgent.SetDestination(dest);
    }


}
