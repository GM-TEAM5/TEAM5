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


    public override string id => "2003";
    public override string dataName => "DashSkill";


    protected override void OnEquip()
    {
        // Debug.Log("DashSkill is equipped!");
    }

    protected override void OnUnEquip()
    {
        // Debug.Log("DashSkill is unequipped!");
    }

    public override void Use()
    {
        Player.Instance.StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        // 대시 실행
        float originalMultiplier = Player.Instance.status.movementSpeedMultiplier;
        Player.Instance.status.movementSpeedMultiplier *= dashMultiplier;

        // 대시 지속
        yield return new WaitForSeconds(dashDuration);
        Player.Instance.status.movementSpeedMultiplier = originalMultiplier;
    }
}