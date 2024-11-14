using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor.Rendering;
using Unity.Collections;


[CreateAssetMenu(fileName = "PlayerData", menuName = "SO/Player/PlayerData", order = int.MaxValue)] 
public class PlayerDataSO : ScriptableObject
{
    public int traitPoint;
    public int currChapter = 1;
    public int currStage = 1;
    public int currStagePlayCount;
    public int deathCount;
    

    public bool isNewUser => deathCount==0 && currChapter ==1 && currStage ==1;

    [Min(0)]public int maxCount_equipment = 8;
    [Min(0)]public int maxCount_skill = 4;


    public List< EquipmentItemSO > equipments;
    public List< SkillItemSO > skills;


    //==================================================================================

    void OnValidate()
    {
        FixContainerSize();
    }

    //==================================================================================

    /// <summary>
    /// 리스트의 최대 크기 지정 - 장비와 스킬
    /// </summary>
    void FixContainerSize()
    {
        // 장비
        equipments = equipments
        .Take( maxCount_equipment)
        .Concat(Enumerable.Repeat( (EquipmentItemSO)null, Mathf.Max(0, maxCount_equipment - equipments.Count)))  // 부족한 부분을 0으로 채움
        .ToList();

        // 스킬
        skills = skills
        .Take( maxCount_skill) 
        .Concat(Enumerable.Repeat( (SkillItemSO)null, Mathf.Max(0,maxCount_skill - skills.Count)))  // 부족한 부분을 0으로 채움
        .ToList();
    }
    

    //==================================================================================


    /// <summary>
    /// 장비 장착 시도
    /// </summary>
    /// <param name="equipment"></param>
    /// <returns></returns> 장비 장착에 성공했는지.
    // public bool TryEquipGear(EquipmentItemSO equipment)
    // {
    //     int idx = equipments.FindIndex( x=>x==null);
        
    //     if( idx != -1)
    //     {
    //         equipments[idx]  = equipment;
    //         Player.Instance.Equip( idx, equipment);
            
    //         return true;
    //     }
    
    //     return false;
    // }


    /// <summary>
    /// 스킬 장착 시도
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns> 스킬 장착에 성공했는지. 
    public bool TryEquipSkill(SkillItemSO skill)
    {
        int idx = skills.FindIndex( x=>x==null);
        
        if( idx != -1)
        {
            skills[idx]  = skill;
            Player.Instance.ChangeSkill(idx,skill);
            
            return true;
        }
    
        return false;
    }
}
