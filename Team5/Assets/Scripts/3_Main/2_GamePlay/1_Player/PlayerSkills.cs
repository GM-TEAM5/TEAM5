using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    //--------- equipments ------------
    // public SerializableDictionary<int, PlayerEquipment> equipments;
    public List<PlayerSkill> passives;
    public SerializableDictionary<SkillType, PlayerSkill> actives;

    // public skill_
    // public PlayerSkill draw;
    // public PlayerSkill util;
    // public PlayerSkill scroll;


    public PlayerSkill currAction;  // 현재 실행할 액션


   // 데이터 상의 모든 스킬장착
    public void Init()
    {        
        actives = new();
        foreach(var kv in GameManager.Instance.playerData.ability_actives )
        {  
            PlayerSkill playerSkill = new(kv.Value);
            actives[kv.Key] =  playerSkill;
            kv.Value?.Equip();
        }  
    }

    //===========================================

    /// <summary>
    /// 입력을 감지하고, 그에 맞는 스킬 실행
    /// </summary>
    public void OnUpdate()
    {
        SetCurrAction();
        TryUse(); 
    }   


    /// <summary>
    ///  플레이어 입력에 따라 현재 사용할 능력을 지정한다. 
    /// </summary>
    void SetCurrAction()
    {
        
        if (PlayerInputManager.Instance.util)
        {
            currAction = actives[SkillType.Util];
        }
        else if (PlayerInputManager.Instance.basicAttack)
        {
            currAction = actives[SkillType.BasicAttack];
        }
        else if (PlayerInputManager.Instance.draw)
        {
            currAction = actives[SkillType.Draw];
        }
        else if (PlayerInputManager.Instance.scroll)
        {
            currAction = actives[SkillType.Scroll];
        }
        else
        {
            currAction = null;
        }
    }


    void TryUse()
    {
        if (currAction ==null)
        {
            return;
        }

        if (currAction.CanUse())
        {
            currAction.Use();
        }

    }

    //==============================================================

    public bool HasEmptySpace(SkillType skillType)
    {
        if (skillType == SkillType.Passive)
        {
            return true;
        }
        else 
        {
            return actives[skillType] == null;
        }
    }

    /// <summary>
    /// 스킬 교체 
    /// </summary>
    /// <param name="skillData"></param>
    public void SwitchSkill(SkillItemSO skillData)
    {
        SkillType skillType = skillData.skillType;
        
        PlayerSkill currSkill = actives[skillType];
        currSkill.skillData?.UnEquip();
        
        actives[skillType].Init(skillData);
        skillData?.Equip();
    }
}
