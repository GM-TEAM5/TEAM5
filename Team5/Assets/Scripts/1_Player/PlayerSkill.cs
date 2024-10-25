using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill
{
    public PlayerSkillSO skillData;

    public PlayerSkill(PlayerSkillSO skillData)
    {
        this.skillData = skillData;
    }

    public void On()
    {
        skillData.On();
    }

    public void Off()
    {
        skillData.Off();
    }

    public void Use(bool isMouseLeftButtonOn, Vector3 mouseWorldPos)
    {
        skillData.Use(isMouseLeftButtonOn, mouseWorldPos);
    }
}
