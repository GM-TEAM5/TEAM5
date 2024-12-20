using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "4000", menuName = "SO/SkillItem/4000_Dash", order = int.MaxValue)]
public class ItemSO_Dash : SkillItemSO
{
    [Header("Dash Settings")]
    [SerializeField] private float dashMultiplier = 3f;
    [SerializeField] private float dashDuration = 0.2f;

    [Header("Trail Settings")]
    [SerializeField] private Color trailStartColor = new Color(0.2f, 0.5f, 1f, 1f);  // 더 진한 파란색, 완전 불투명
    [SerializeField] private Color trailEndColor = new Color(0.2f, 0.5f, 1f, 0.9f);  // 끝부분도 더 진하고 덜 투명하게
    [SerializeField] private float trailStartWidth = 1.5f;
    [SerializeField] private float trailEndWidth = 0.8f;

    public override string id => $"4000_{property}";
    public override string dataName =>  $"DashSkill_{property}";

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
        TrailRenderer dashTrail = Player.Instance.GetComponentInChildren<TrailRenderer>(true);
        if (dashTrail != null)
        {
            dashTrail.gameObject.SetActive(true);
            dashTrail.enabled = true;
            dashTrail.Clear();

            // 트레일 색상 설정
            dashTrail.startColor = trailStartColor;
            dashTrail.endColor = trailEndColor;

            // 트레일 너비 설정
            dashTrail.startWidth = trailStartWidth;
            dashTrail.endWidth = trailEndWidth;

            // 트레일이 더 선명하게 보이도록 시간 설정 조정
            dashTrail.time = 0.2f;  // 트레일이 사라지는 시간을 좀 더 길게
            dashTrail.minVertexDistance = 0.1f;  // 더 부드러운 트레일을 위한 설정
        }

        // 대시 실행
        float originalMultiplier = Player.Instance.status.movementSpeedMultiplier;
        Player.Instance.status.movementSpeedMultiplier *= dashMultiplier;

        // 대시 지속
        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        // 원래 속도로 복구
        Player.Instance.status.movementSpeedMultiplier = originalMultiplier;

        // 잔상이 빠르게 사라지도록 짧은 딜레이 후 비활성화
        yield return new WaitForSeconds(0.2f);
        if (dashTrail != null)
        {
            dashTrail.enabled = false;
            dashTrail.gameObject.SetActive(false);
        }
    }
}