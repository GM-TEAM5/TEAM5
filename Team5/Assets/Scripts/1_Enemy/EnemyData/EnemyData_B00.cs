using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData_B00", menuName = "SO/Enemy/B00", order = int.MaxValue)]
public class EnemyData_B00 : EnemyDataSO
{
    public override void OnInit(Enemy enemy)
    {
        GamePlayManager.Instance.UpdateEnemyHpSlider(enemy);
    }
    
    
    public override void OnAttack(Enemy enemy)
    {
        
    }

    public override void OnDie(Enemy enemy)
    {
        
    }

    public override void OnHit(Enemy enemy)
    {
        GamePlayManager.Instance.UpdateEnemyHpSlider(enemy);
    }
}
