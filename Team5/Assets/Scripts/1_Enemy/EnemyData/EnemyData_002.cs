using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData_002", menuName = "SO/Enemy/002", order = int.MaxValue)]
public class EnemyData_002 : EnemyDataSO
{
    public override void OnInit(Enemy enemy)
    {
        
    }
    
    public override void OnAttack(Enemy enemy)
    {
        
    }

    public override void OnDie(Enemy enemy)
    {
        // enemy.GetKnockback(20,enemy.lastHitPoint);
    }

    public override void OnHit(Enemy enemy)
    {
        
    }
}
