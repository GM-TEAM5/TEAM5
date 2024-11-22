using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW.Util;

[CreateAssetMenu(fileName = "EA_B0000", menuName = "SO/EnemyAbility/B0000")]
public class EnemyAbility_B0000 : EnemyAbilitySO
{
      [Header("Extra")]
    public float radius = 4;
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
                player.GetImpulsiveDamaged( AbilityDmg(enemy), enemy.t.position, targetPos, impulse);
            }
        }
    }

    public override void StartCast(Enemy enemy)
    {
        Vector3 targetPos = enemy.t_target.position;
    
        PoolManager.Instance.GetAreaIndicator_Circle(targetPos, Vector2.one * radius * 2, castingTime );

        // 타겟 뒤로 순간이동
        enemy.transform.position = targetPos + new Vector3( Random.Range(-1,1),0,Random.Range(-1,1)).normalized * 2f;
        PoolManager.Instance.GetText( targetPos, "!!");

    }
}