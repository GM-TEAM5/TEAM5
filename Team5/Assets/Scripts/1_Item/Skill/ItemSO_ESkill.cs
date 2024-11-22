using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SkillItem_2002_E", menuName = "SO/SkillItem/2002_ESkill", order = int.MaxValue)]
public class ItemSO_ESkill : SkillItemSO, IDrawableSkill
{
    [Header("Drawing Settings")]
    [SerializeField] private Color _lineColor = Color.black;
    [SerializeField] private float _lineWidth = 0.2f;
    [SerializeField] private Material _lineMaterial;
    [SerializeField] private float _effectRadius = 1f;
    [SerializeField] private float _lineDuration = 3f;

    [Header("Skill Settings")]
    public float slowFieldDuration = 3f;
    public float slowDuration = 2f;
    public float slowAmount = 0.5f;
    public float damage = 25f;

    [Header("Ink Settings")]
    [SerializeField] private float _inkCostPerUnit = 1f;
    [SerializeField] private float _minInkRequired = 10f;

    [Header("Duration Settings")]
    [SerializeField] private float maxDrawDuration = 5f;  // 최대 드로잉 시간
    private float drawTimer;
    private Coroutine drawTimerCoroutine;

    public override string id => "2002";
    public override string dataName => "ESkill";

    // IDrawableSkill 구현
    Color IDrawableSkill.lineColor => _lineColor;
    float IDrawableSkill.lineWidth => _lineWidth;
    Material IDrawableSkill.lineMaterial => _lineMaterial;
    float IDrawableSkill.effectRadius => _effectRadius;
    float IDrawableSkill.inkCostPerUnit => _inkCostPerUnit;
    float IDrawableSkill.minInkRequired => _minInkRequired;

    PlayerDraw playerDraw;

    public override void OnEquip()
    {
        playerDraw = Player.Instance.GetComponentInChildren<PlayerDraw>();
    }

    public override void OnUnEquip()
    {
        playerDraw = null;
        if (drawTimerCoroutine != null)
        {
            Player.Instance.StopCoroutine(drawTimerCoroutine);
        }
    }

    public override void Use()
    {
        if (playerDraw.isInDrawMode)
        {
            playerDraw.FinishDraw();
        }
        else
        {
            drawTimer = maxDrawDuration;
            playerDraw.StartDrawing(
                DrawType.GroundPattern,
                this,
                (line, positions) => OnDrawComplete(line, positions)
            );

            if (drawTimerCoroutine != null)
            {
                Player.Instance.StopCoroutine(drawTimerCoroutine);
            }
            drawTimerCoroutine = Player.Instance.StartCoroutine(DrawTimerRoutine());
        }
    }

    private IEnumerator DrawTimerRoutine()
    {
        while (drawTimer > 0)  // playerDraw.isDrawing 조건 제거
        {
            drawTimer -= Time.deltaTime;
            yield return null;
        }

        playerDraw.FinishDraw();
        drawTimerCoroutine = null;
    }

    private void OnDrawComplete(LineRenderer line, List<Vector3> positions)
    {
        Player.Instance.StartCoroutine(SlowFieldRoutine(line, positions));
    }

    IEnumerator SlowFieldRoutine(LineRenderer line, List<Vector3> positions)
    {
        float elapsedTime = 0f;
        var drawableSkill = this as IDrawableSkill;
        HashSet<Enemy> damagedEnemies = new HashSet<Enemy>();

        // 초기 데미지 판정 (한 번만)
        for (int i = 0; i < positions.Count - 1; i++)
        {
            RaycastHit[] hits = Physics.SphereCastAll(
                positions[i],
                drawableSkill.effectRadius,
                Vector3.up,
                0.1f,
                LayerMask.GetMask("Enemy")
            );

            foreach (var hit in hits)
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null && !damagedEnemies.Contains(enemy))
                {
                    float dmg = damage + Player.Instance.status.mDmg;
                    enemy.GetDamaged(dmg );
                    damagedEnemies.Add(enemy);
                }
            }
        }

        // 지속 효과 (슬로우)
        while (elapsedTime < _lineDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float completion = elapsedTime / _lineDuration;

            // 슬로우 효과 적용
            for (int i = 0; i < positions.Count - 1; i++)
            {
                RaycastHit[] hits = Physics.SphereCastAll(
                    positions[i],
                    drawableSkill.effectRadius,
                    Vector3.up,
                    0.1f,
                    LayerMask.GetMask("Enemy")
                );

                foreach (var hit in hits)
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.ApplySlow(slowAmount, slowDuration);
                    }
                }
            }

            // 라인 페이드 아웃
            Color color = line.startColor;
            color.a = 1 - completion;
            line.startColor = line.endColor = color;

            yield return null;
        }

        UnityEngine.Object.Destroy(line.gameObject);
    }
}