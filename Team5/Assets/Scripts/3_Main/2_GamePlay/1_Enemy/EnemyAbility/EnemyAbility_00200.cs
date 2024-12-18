using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;

[CreateAssetMenu(fileName = "EA_00200", menuName = "SO/EnemyAbility/00200")]
public class EnemyAbility_00200 : EnemyAbilitySO
{
    [Header("Extra")]
    
    public float movementSpeed;
    public float lifeTime;
    

    //==================================================================================
    public override bool ActivationConditions(Enemy enemy)
    {
        return true;
    }
    public override bool UsageConditions(Enemy enemy)
    {
        bool inRange = enemy.targetDistSqr <= range * range;
        return inRange;
    }

    public override void ApplyAbility(Vector3 castingPos,Enemy enemy)
    {
        Vector3 initPos = enemy.t.position.WithStandardHeight();
        Vector3 targetPos = castingPos.WithStandardHeight();
        
        EnemyProjectile enemyProjectile = PoolManager.Instance.GetEnemyProjectile(this, enemy,initPos, lifeTime);
        Vector3 dir = (targetPos - initPos).normalized;
        enemyProjectile.SetDirAndSpeed(dir,movementSpeed); // 날라갈수있게 세팅
    }

    public override void StartCast(Enemy enemy)
    {
        
    }
}
