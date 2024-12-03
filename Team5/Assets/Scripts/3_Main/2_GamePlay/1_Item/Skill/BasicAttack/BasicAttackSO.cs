using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;

public abstract class BasicAttackSO : SkillItemSO
{
    [Header("BasicAttack Setting")]
    public float defaultDamage;
    public float damageWeight;

    public int comboCount;
    public float comboResetTime = 1f;

    public List<GameObject> effects;        // 각 모션의 이펙트 
    public List<AnimationClip> animations;  // 각 모션의 애니메이션
    public List<float> delays;              // 다음 모션까지의 딜레이
    // public List<Action> attackEffects;      // 공격 적용 판정 (SO로 뺼거임)



    public BasicAttackSO()
    {
        skillType = SkillType.BasicAttack;
    }
    
    
    //===============================================
    void OnValidate()
    {
        //
        FixListCap(ref effects);
        FixListCap(ref animations);
        FixListCap(ref delays);
        // FixListCap(ref attackEffects);
    }

    // 콤보카운트에 맞춰 리스트의 크기를 조절한다.     
    void FixListCap<T>(ref List<T> list)
    {
        list = list
        .Take(comboCount)
        .Concat(Enumerable.Repeat(default(T), Mathf.Max(0, comboCount - list.Count)))  // 부족한 부분을 0으로 채움
        .ToList();
    }


    //==========================================================

    protected override void OnEquip()
    {
        // 플레이어 계층 구조 하위에 지정된 이펙트 생성
    }

    protected override void OnUnEquip()
    {
        // 플레이어 계층 구조 하위에 생성되었던 이펙트 파괴
    }


    
    public override void Use()
    {
        //Player BasicAttack 함수 실행. 
    }
}
