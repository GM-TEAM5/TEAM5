using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public interface IDrawableSkill
{
    Color lineColor { get; }
    float lineWidth { get; }
    Material lineMaterial { get; }
    float effectRadius { get; }
    float inkCostPerUnit { get; }
    float minInkRequired { get; }
}

[CreateAssetMenu(fileName = "SkillItem_2001_Q", menuName = "SO/SkillItem/2001_QSkill", order = int.MaxValue)]
public class ItemSO_QSkill : SkillItemSO, IDrawableSkill
{
    [Header("Drawing Settings")]
    [SerializeField] private Color _lineColor = Color.black;
    [SerializeField] private float _lineWidth = 0.3f;
    [SerializeField] private Material _lineMaterial;
    [SerializeField] private float _effectRadius = 1f;

    [Header("Skill Settings")]
    public float slashDuration = 0.5f;
    public float damage = 30f;

    [Header("Ink Settings")]
    [SerializeField] private float _inkCostPerUnit = 2f;
    [SerializeField] private float _minInkRequired = 15f;

    public override string id => "2001";
    public override string dataName => "QSkill";
    public new Color lineColor => _lineColor;
    public new float lineWidth => _lineWidth;
    public new Material lineMaterial => _lineMaterial;
    public new float effectRadius => _effectRadius;
    public new float inkCostPerUnit => _inkCostPerUnit;
    public new float minInkRequired => _minInkRequired;

    PlayerDraw playerDraw;

    public override void OnEquip()
    {
        playerDraw = Player.Instance.GetComponentInChildren<PlayerDraw>();
    }

    public override void OnUnEquip()
    {
        playerDraw = null;
    }

    public override void Use()
    {
        if (playerDraw.isDrawing)
        {
            playerDraw.FinishDraw();
        }
        else
        {
            playerDraw.StartDrawing(
                DrawType.QuickSlash,
                this,
                (line, positions) => OnDrawComplete(line, positions)
            );
        }
    }

    private void OnDrawComplete(LineRenderer line, List<Vector3> positions)
    {
        Player.Instance.StartCoroutine(SlashRoutine(line, positions));
    }

    IEnumerator SlashRoutine(LineRenderer line, List<Vector3> positions)
    {
        float elapsedTime = 0f;
        Color startColor = line.startColor;
        var drawableSkill = this as IDrawableSkill;
        HashSet<Enemy> damagedEnemies = new HashSet<Enemy>();

        while (elapsedTime < slashDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = 1f - (elapsedTime / slashDuration);
            Color fadeColor = startColor;
            fadeColor.a = alpha;
            line.startColor = line.endColor = fadeColor;

            // 데미지 판정
            for (int i = 0; i < positions.Count - 1; i++)
            {
                RaycastHit[] hits = Physics.BoxCastAll(
                    positions[i],
                    Vector3.one * drawableSkill.effectRadius,
                    (positions[i + 1] - positions[i]).normalized,
                    Quaternion.identity,
                    Vector3.Distance(positions[i], positions[i + 1]),
                    LayerMask.GetMask("Enemy")
                );

                foreach (var hit in hits)
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null && !damagedEnemies.Contains(enemy))
                    {
                        enemy.GetDamaged(damage);
                        damagedEnemies.Add(enemy);
                    }
                }
            }

            yield return null;
        }

        UnityEngine.Object.Destroy(line.gameObject);
    }
}
