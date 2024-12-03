using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public enum SkillType
{
    Passive,
    BasicAttack,
    Draw,
    Util,
    Scroll
}

/// <summary>
/// 스킬은 장착 아이템이면서도, 사용 효과가 있는 아이템임. 
/// </summary>
public abstract class SkillItemSO : ItemDataSO
{
    public SkillType skillType;
    
    [Min(1)] public float coolTime = 1;

    
    public SkillItemSO()
    {
        type = ItemType.Skill;
    }

    protected override bool CanGet(out CantGetReason reason)
    {        
        reason = CantGetReason.None;

        if (Player.Instance.skills.HasEmptySpace(skillType)==false)
        {
            reason = CantGetReason.NoSpace;
        }
        return true;
    }


    protected override void Get()
    {
        Player.Instance.skills.SwitchSkill(this);

    }

    protected override void OnCantGet(CantGetReason reason)
    {
        switch(reason)
        {
            case CantGetReason.NoSpace:
                // GamePlayManager.Instance.OnInventoryFull(this);
                Debug.Log("장착할 스킬이 없음");
                break;
        }
    }



    public void Equip()
    {
        OnEquip();  // 장착효과
    }


    public void UnEquip()
    {
        //플레이어 장비창에서 사라짐. 획득 능력치는삭제

        OnUnEquip(); // 장착효과 해제
    }


    protected abstract void OnEquip();
    protected abstract void OnUnEquip();

    public abstract void Use();
}
