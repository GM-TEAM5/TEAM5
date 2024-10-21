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
        enemy.StartCoroutine( MeleeAttack(enemy,targetPos ));
    }

    IEnumerator MeleeAttack(Enemy enemy, Vector3 targetPos)
    {
        yield return new WaitForSeconds(0.2f);  //근접공격 무빙으로 피할수도있음.
        if(enemy.isAlive)
        {
            EnemyProjectile enemyProjectile = PoolManager.Instance.GetEnemyProjectile(this, enemy, targetPos.WithStandardHeight(), lifeTime);
        }
        
    }
}
