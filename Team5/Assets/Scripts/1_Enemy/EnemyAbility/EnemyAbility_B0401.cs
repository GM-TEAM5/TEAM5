using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW.Util;



[CreateAssetMenu(fileName = "EA_B0001", menuName = "SO/EnemyAbility/B0001")]        //돌진
public class EnemyAbility_B0401 :  EnemyAbilitySO
{
    [Header("Extra")]
    public float radiusWeight = 1.3f;
    public float movementSpeed;
    public float impulse = 50;
        
    public float rushDuration = 1f; // 돌진 지속 시간



    public override bool ActivationConditions(Enemy enemy)
    {
        return true;
    }

    public override bool UsageConditions(Enemy enemy)
    {
        bool inRange = enemy.targetDistSqr <= range * range;
        return inRange;
    }



    public override void StartCast(Enemy enemy)
    {
        // Vector3 targetPos = enemy.t_target.position;
        CapsuleCollider capsuleCollider = enemy.GetComponent<CapsuleCollider>();
        PoolManager.Instance.GetAreaIndicator_Circle(enemy.t.position, Vector2.one * capsuleCollider.radius*radiusWeight *2, castingTime );
    }

    
    public override void ApplyAbility(Vector3 castingPos, Enemy enemy)
    {
        Vector3 targetPos = castingPos;

        enemy.StartCoroutine( RushRoutine(enemy));
    }



    //===========================================
    private IEnumerator RushRoutine( Enemy enemy)
    {
        
        float dmg = AbilityDmg(enemy);
        CapsuleCollider capsuleCollider = enemy.GetComponent<CapsuleCollider>();
        
        // 캡슐 콜라이더의  좌표 계산
        
        float halfHeight = Mathf.Max(0, (capsuleCollider.height / 2) - capsuleCollider.radius);

        

        // 돌진 시작
        Vector3 startPos = enemy.transform.position;
        Vector3 targetPos = enemy.t_target.position;
        Vector3 direction = (targetPos - startPos).normalized;

        //
        bool playerHit = false;
        float elapsedTime  = 0;
        while(elapsedTime < rushDuration)
        {
            if( enemy.isAlive ==false)
            {
                yield break;
            }
            
            enemy.ai.navAgent.Move(direction * movementSpeed * Time.deltaTime);     
            
            // 플레이어에게 한번의 공격을 입힘. 
            if (playerHit ==false)
            {
                Vector3 center = enemy.t.position;
                Vector3 point1 = center + Vector3.up * halfHeight;
                Vector3 point2 = center - Vector3.up * halfHeight;
            
                // 충돌검사 및 피해    
                Collider[] hits = Physics.OverlapCapsule(point1, point2, capsuleCollider.radius *radiusWeight *1.5f,GameConstants.playerLayer);
                if(hits.Length>0)
                {
                    Collider hit = hits[0];

                    // 적에게 피해를 입히는 로직
                    if (hit.CompareTag("Player"))
                    {
                        Player.Instance.GetImpulsiveDamaged( dmg, center, hit.ClosestPoint(enemy.t_target.position), impulse);
                        playerHit  = true;
                    }
                }
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }

}
