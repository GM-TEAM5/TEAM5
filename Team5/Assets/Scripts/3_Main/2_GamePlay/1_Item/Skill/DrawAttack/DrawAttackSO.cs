using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class DrawAttackSO : SkillItemSO
{
    [Header("Drawing Settings")]
    [SerializeField] protected Color _lineColor = Color.black;
    [SerializeField] protected float _lineWidth = 0.3f;
    [SerializeField] protected Material _lineMaterial;
    [SerializeField] protected float _effectRadius = 1f;

    [Header("Skill Settings")]
    public float slashDuration = 0.5f;
    public float defaultDamage;
    public float damageWeight;

    [Header("Ink Settings")]
    [SerializeField] protected float _inkCostPerUnit = 2f;
    [SerializeField] protected float _minInkRequired = 15f;

    [Header("Duration Settings")]
    [SerializeField] protected float maxDrawDuration = 8f;

    public Color lineColor => _lineColor;
    public float lineWidth => _lineWidth;
    public Material lineMaterial => _lineMaterial;
    public float effectRadius => _effectRadius;
    public float inkCostPerUnit => _inkCostPerUnit;
    public float minInkRequired => _minInkRequired;

    protected PlayerDraw playerDraw;

    public DrawAttackSO()
    {
        skillType = SkillType.Draw;
    }

    protected override void OnEquip()
    {
        playerDraw = Player.Instance.GetComponentInChildren<PlayerDraw>();
    }

    protected override void OnUnEquip()
    {
        playerDraw = null;
    }

    public override void Use()
    {
        if (playerDraw.isInDrawMode)
        {
            playerDraw.FinishDraw();
        }
        else
        {
            playerDraw.StartDrawing(
                this,
                (line, positions) => OnDrawComplete(line, positions)
            );
        }
    }

    protected virtual void OnDrawComplete(LineRenderer line, List<Vector3> positions)
    {
        Player.Instance.StartCoroutine(SlashRoutine(line, positions));
    }

    protected IEnumerator SlashRoutine(LineRenderer line, List<Vector3> positions)
    {
        float elapsedTime = 0f;
        Color startColor = line.startColor;
        HashSet<Enemy> damagedEnemies = new HashSet<Enemy>();

        while (elapsedTime < slashDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = 1f - (elapsedTime / slashDuration);
            Color fadeColor = startColor;
            fadeColor.a = alpha;
            line.startColor = line.endColor = fadeColor;

            for (int i = 0; i < positions.Count - 1; i++)
            {
                RaycastHit[] hits = Physics.BoxCastAll(
                    positions[i],
                    Vector3.one * effectRadius,
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
                        float dmg = defaultDamage + Player.Instance.status.mDmg;
                        enemy.GetDamaged(dmg);
                        damagedEnemies.Add(enemy);
                    }
                }
            }

            yield return null;
        }

        UnityEngine.Object.Destroy(line.gameObject);
    }
}
