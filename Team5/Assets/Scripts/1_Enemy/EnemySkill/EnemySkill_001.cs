using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

using BW.Util;

[CreateAssetMenu(fileName = "eSkill_001_Fire", menuName = "SO/enemySkill/001")]
public class EnemySkill_001 : EnemySkillSO
{
    [Header("Extra")]
    public float movementSpeed;
    

    //==========
    
    public override void Use(Enemy enemy, Vector3 targetPos)
    {
        Vector3 initPos = enemy.transform.position.WithStandardHeight();
        
        EnemyProjectile enemyProjectile = PoolManager.Instance.GetEnemyProjectile(this, enemy,initPos, lifeTime);
        Vector3 dir = (targetPos.WithStandardHeight() - initPos).normalized;
        enemyProjectile.SetDirAndSpeed(dir,movementSpeed); // 날라갈수있게 세팅
    }
    //
}
