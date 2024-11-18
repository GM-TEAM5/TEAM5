using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW.Util;



[CreateAssetMenu(fileName = "EA_00300", menuName = "SO/EnemyAbility/00300")]
public class EnemyAbility_00300 :  EnemyAbilitySO
{
    [Header("Extra")]
    public float radius = 3;
    public float impulse = 20;
    
    
    public override bool ActivationConditions(Enemy enemy)
    {
        return true;
    }

    public override bool UsageConditions(Enemy enemy)
    {
        bool inRange = enemy.targetDistSqr <= range * range;
        return inRange;
    }

    public override void ApplyAbility(Vector3 castingPos, Enemy enemy)
    {
        Vector3 targetPos = castingPos;

        Collider[] hits = Physics.OverlapSphere(targetPos.WithStandardHeight(), radius,GameConstants.playerLayer);

        // 충돌지역에 플레이어가 있으면. 
        if(hits.Length>0)
        {
            Collider hit = hits[0];

            // 적에게 피해를 입히는 로직
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                player.GetImpulsiveDamaged(enemy.data.ad, enemy.t.position, targetPos, impulse);
            }
        }
    }

    public override void StartCast(Enemy enemy)
    {
        Vector3 targetPos = enemy.t_target.position;
        
        PoolManager.Instance.GetAreaIndicator_Circle(targetPos, Vector2.one * radius * 2, castingTime );
    }
}
