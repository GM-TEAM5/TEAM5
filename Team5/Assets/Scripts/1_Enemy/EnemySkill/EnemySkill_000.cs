using System.Collections;
using System.Collections.Generic;
using BW.Util;
using UnityEngine;

[CreateAssetMenu(fileName = "eSkill_000_MeleeAttack", menuName = "SO/enemySkill/000")]
public class EnemySkill_000 : EnemySkillSO
{
    //
    public override void Use(Enemy enemy, Vector3 targetPos)
    {
        Vector3 dir = (targetPos - enemy.t.position).WithFloorHeight().normalized;
        float radius = 1;

        Collider[] hits = Physics.OverlapSphere(targetPos.WithStandardHeight(), radius,GameConstants.playerLayer);

        // 충돌된 오브젝트들에 대해 반복 실행
        if(hits.Length>0)
        {
            Collider hit = hits[0];

            // 적에게 피해를 입히는 로직
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                player.GetDamaged(enemy.enemyData.ad);
            }
        }
    }

    // IEnumerator MeleeAttack(Enemy enemy, Vector3 targetPos)
    // {
    //     yield return new WaitForSeconds(0.2f);  //근접공격 무빙으로 피할수도있음.
    //     if(enemy.isAlive)
    //     {
    //         EnemyProjectile enemyProjectile = PoolManager.Instance.GetEnemyProjectile(this, enemy, targetPos.WithStandardHeight(), lifeTime);
    //     }
        
    // }
}
