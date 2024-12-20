using UnityEngine;
using System.Collections;

public abstract class DashSO : SkillItemSO
{
    [Header("Dash Settings")]
    [SerializeField] protected float dashMultiplier = 3f;
    [SerializeField] protected float dashDuration = 0.2f;

    [Header("Trail Settings")]
    [SerializeField] protected Color trailStartColor = new Color(0.2f, 0.5f, 1f, 1f);
    [SerializeField] protected Color trailEndColor = new Color(0.2f, 0.5f, 1f, 0.7f);
    [SerializeField] protected float trailStartWidth = 1.5f;
    [SerializeField] protected float trailEndWidth = 0.8f;
    [SerializeField] protected float trailTime = 0.2f;
    [SerializeField] protected float minVertexDistance = 0.1f;

    protected DashSO()
    {
        skillType = SkillType.Util;
    }

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

    protected virtual IEnumerator DashRoutine()
    {
        TrailRenderer dashTrail = Player.Instance.GetComponentInChildren<TrailRenderer>(true);
        if (dashTrail != null)
        {
            dashTrail.gameObject.SetActive(true);
            dashTrail.enabled = true;
            dashTrail.Clear();

            // 트레일 색상과 너비 설정
            dashTrail.startColor = trailStartColor;
            dashTrail.endColor = trailEndColor;
            dashTrail.startWidth = trailStartWidth;
            dashTrail.endWidth = trailEndWidth;

            // 끝부분을 둥글게 만드는 설정
            dashTrail.numCornerVertices = 8;
            dashTrail.numCapVertices = 8;
            dashTrail.minVertexDistance = 0.1f;

            // 트레일이 더 부드럽게 보이도록 설정
            dashTrail.shadowBias = 0.5f;
            dashTrail.generateLightingData = true;
            dashTrail.time = trailTime;
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