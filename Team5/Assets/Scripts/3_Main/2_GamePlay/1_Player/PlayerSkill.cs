using System;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerSkill
{    
    public SkillItemSO skillData;

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
