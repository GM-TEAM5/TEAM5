using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class PlayerSkillSO  : ScriptableObject 
{
    public string id;
    public string skillName;
    public Sprite icon;
    //
    public float cooltime;      //쿨타임 ( 초 ) 

    public abstract Vector3 FindTargetPos();
    

    public abstract void Use(Vector3 targetPos);
}