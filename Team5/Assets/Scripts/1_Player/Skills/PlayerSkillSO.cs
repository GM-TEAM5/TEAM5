using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PlayerSkillSO : ScriptableObject
{
    public string id;
    public string skillName;
    public Sprite icon;

    public abstract void On();
    public abstract void Off();
    public abstract void Use(bool isMouseLeftButtonOn, Vector3 mouseWorldPos);
}