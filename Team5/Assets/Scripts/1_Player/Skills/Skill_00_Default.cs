using UnityEngine;

[CreateAssetMenu(fileName = "Skill_00_Default", menuName = "SO/PlayerSkill/00")]
public class Skill_00_Default : PlayerSkillSO
{
    // float lastMeleeAttackTime;
    // bool meleeAttackOk => Time.time > lastMeleeAttackTime + status.attackSpeed;
    // int combo = 0;

    // // 마지막으로 캐스팅한 방향 저장
    // private Vector3 lastCastDirection;

    // // 현재 캐스팅 중인지 여부
    // private bool debug_normalAttack = false;

    public override void On()
    {
    }

    public override void Off()
    {
    }

    public override void Use(bool isMouseLeftButtonOn, Vector3 mouseWorldPos)
    {
        // MeleeAttack(mouseWorldPos);
    }

    // // 좌클릭시 근접공격
    // // 1, 2타: 찌르기
    // // 3타 : 베기
    // void MeleeAttack(Vector3 mouseWorldPos)
    // {
    //     if (meleeAttackOk == false)
    //     {
    //         return;
    //     }

    //     lastMeleeAttackTime = Time.time;
    //     bool isEnhancedAttack = ++combo == 3;

    //     if (isEnhancedAttack)
    //     {
    //         combo = 0;

    //         // 강화 후엔 딜레이 좀 두려고
    //         lastMeleeAttackTime += status.attackSpeed * 2;
    //         MeleeAttack_Enhanced(mouseWorldPos);
    //     }
    //     else
    //     {
    //         MeleeAttack_Normal(mouseWorldPos);
    //     }
    // }

    // // 일반공격 - 좁은 범위를 찌른다.
    // void MeleeAttack_Normal(Vector3 mouseWorldPos)
    // {
    //     Debug.Log("일반공격");

    //     Vector3 dir = (mouseWorldPos - t_player.position).WithFloorHeight().normalized;
    //     float radius = 1;
    //     float maxDist = 5;

    //     RaycastHit[] hits = Physics.SphereCastAll(t_player.position.WithStandardHeight(), radius, dir, maxDist, GameConstants.enemyLayer);

    //     // 충돌된 오브젝트들에 대해 반복 실행
    //     for (int i = 0; i < hits.Length; i++)
    //     {
    //         RaycastHit hit = hits[i];

    //         // 적에게 피해를 입히는 로직
    //         Enemy enemy = hit.collider.GetComponent<Enemy>();
    //         if (enemy != null)
    //         {
    //             enemy.GetDamaged(hit.point, status.ad);
    //         }
    //     }


    //     // for debug.
    //     lastCastDirection = dir;
    //     debug_normalAttack = true;
    // }

    // // 강화 공격 - 플레어가 보는 방향의 180도를 휩쓸기 공격을 한다.
    // void MeleeAttack_Enhanced(Vector3 mouseWorldPos)
    // {
    //     Debug.Log("강화공격!!!!!");

    //     Vector3 mouseDir = (mouseWorldPos - t_player.position).WithFloorHeight().normalized;

    //     // OverlapSphere를 사용해 모든 적을 반경 내에서 감지
    //     float maxDist = 8;
    //     Collider[] hitColliders = Physics.OverlapSphere(t_player.position.WithStandardHeight(), maxDist, GameConstants.enemyLayer);

    //     for (int i = 0; i < hitColliders.Length; i++)
    //     {
    //         Collider hitCollider = hitColliders[i];


    //         // 방향 벡터 계산 (origin에서 적으로)
    //         Vector3 enemyDir = (hitCollider.transform.position - t_player.position).normalized;
    //         float angleWithEnemy = Vector3.Angle(mouseDir, enemyDir);

    //         // 각도가 설정된 범위 내에 있는지 확인 (90도 이하만 허용 = 반구)
    //         if (angleWithEnemy <= 90)
    //         {
    //             // 적에게 피해를 입힘
    //             Enemy enemy = hitCollider.GetComponent<Enemy>();
    //             if (enemy != null)
    //             {
    //                 enemy.GetDamaged(hitCollider.ClosestPoint(t_player.position), status.ad * 1.5f);
    //             }
    //         }
    //     }
    // }
}
