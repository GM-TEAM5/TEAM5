using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySkillSO : ScriptableObject
{
    public string id;
    public string skillName;
    public Sprite icon;
    //
    public float lifeTime;
    public float cooltime;      //쿨타임 ( 초 ) 

    //
    public abstract void Use(Enemy enemy, Vector3 targetPos);
}
