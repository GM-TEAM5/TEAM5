using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "SkillItem_2003_Dash", menuName = "SO/SkillItem/2003_Dash", order = int.MaxValue)]
public class ItemSO_Dash : SkillItemSO
{
    [Header("Dash Settings")]
    [SerializeField] private float dashMultiplier = 3f;  // 이동 속도 증가 배율
    [SerializeField] private float dashDuration = 0.1f;  // 대시 지속 시간
    [SerializeField] private float cooldown = 1f;        // 쿨타임

    public override string id => "2003";
    public override string dataName => "DashSkill";

    private Player player;
    private float originalMultiplier;
    private bool canUse = true;

    public override void OnEquip()
    {
        player = Player.Instance;
    }

    public override void OnUnEquip()
    {
        player = null;
    }

    public override void Use()
    {
        if (player == null || !canUse) return;

        // 대시 실행
        originalMultiplier = player.status.movementSpeedMultiplier;
        player.status.movementSpeedMultiplier *= dashMultiplier;

        // 쿨타임과 속도 복구 시작
        canUse = false;
        player.StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        // 대시 지속
        yield return new WaitForSeconds(dashDuration);
        player.status.movementSpeedMultiplier = originalMultiplier;

        // 쿨타임
        yield return new WaitForSeconds(cooldown);
        canUse = true;
    }
}