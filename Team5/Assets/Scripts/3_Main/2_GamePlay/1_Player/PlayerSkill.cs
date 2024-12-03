using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerSkill
{    
    public SkillItemSO skillData;

    
    public float lastUseTime;

    
    
    
    //=================================================================
    
    
    public PlayerSkill(SkillItemSO skillData)
    {
        Init(skillData);
    }


    public void Init(SkillItemSO skillData)
    {
        this.skillData = skillData;
    }

    public void Use()
    {
        if (skillData==null)        
        {
            return;
        }
        skillData.Use();
        lastUseTime = Time.time; 
    }
    
    public bool CanUse()
    {
        if (skillData==null)
        {
            return false;
        }
        
        return Time.time >= lastUseTime + skillData.coolTime;
    }

    // public void On()
    // {
    //     skillData.On();
    // }

    // public void Off()
    // {
    //     skillData.Off();
    // }

    // public void Use(bool isMouseLeftButtonOn, Vector3 mouseWorldPos)
    // {
    //     skillData.Use(isMouseLeftButtonOn, mouseWorldPos);
    // }
}
