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

    public DrawAttackSO()
    {
        skillType = SkillType.Draw;
    }

    protected override void OnEquip()
    {
        Player.Instance.draw.Equip(this);
    }

    protected override void OnUnEquip()
    {
        Player.Instance.draw.UnEquip();
    }

    public override void Use()
    {
        Player.Instance.draw.TryDraw();
    }
}
