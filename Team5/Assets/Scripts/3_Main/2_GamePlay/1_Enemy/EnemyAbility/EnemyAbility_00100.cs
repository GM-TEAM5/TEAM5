using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BW;

[CreateAssetMenu(fileName = "EA_00100", menuName = "SO/EnemyAbility/00100")]
public class EnemyAbility_00100 : EnemyAbilitySO
{
    [Header("Extra")]
    public float radius = 1;
    
    
    
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
        Vector3 targetPos = castingPos.WithStandardHeight();
        // Vector3 dir = (targetPos - enemy.t.position).WithFloorHeight().normalized;

        Collider[] hits = Physics.OverlapSphere(targetPos, radius,GameConstants.playerLayer);

        // 충돌지역에 플레이어가 있으면. 
        if(hits.Length>0)
        {
            Collider hit = hits[0];

            // 적에게 피해를 입히는 로직
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                player.GetDamaged(enemy.data.ad);
            }
        }
    }

    public override void StartCast(Enemy enemy)
    {
        
    }
}
