using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;

[CreateAssetMenu(fileName = "DefaultBasicAttackDetection", menuName = "SO/AttackDetection/DefaultBasicAttack", order = int.MaxValue)]
public class AttackDetectionSO_DefaultBasicAttack : AttackDetectionSO
{
    public float attackRange = 2f; //평타 종류별로 2f 는 달라질 수 잇음. 
    public float radius = 1.5f;
    
    public override void Detect(Vector3 attackDir, Vector3 effectPos, BasicAttackSO data)
    {
        // 공격 방향 계산
        Vector3 spherePosition = (attackDir * attackRange).WithStandardHeight();  

        // 데미지 판정 - 
        Collider[] hits = Physics.OverlapSphere(effectPos + spherePosition, radius, GameConstants.enemyLayer);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                float dmg = data.defaultDamage + data.damageWeight * Player.Instance.status.pDmg ;
                enemy.GetDamaged( hit.ClosestPoint( Player.Instance.t.position ), dmg );
            }
        }
    }
}
