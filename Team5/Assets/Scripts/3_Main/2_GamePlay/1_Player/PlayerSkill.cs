using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSkill
{
    KeyCode keyCode;

    public SkillItemSO skillData;

    public PlayerSkill(SkillItemSO skillData)
    {
        this.skillData = skillData;
    }

    public void Use()
    {
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
