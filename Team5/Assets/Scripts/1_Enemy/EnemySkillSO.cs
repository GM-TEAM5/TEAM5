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
    public bool interruptionDefense;

    //
    public float delay_beforeCast;  // 선딜 : 공격 기술이 
    public float delay_afterCast;   // 후딜

    //
    public abstract void Use(Enemy enemy, Vector3 targetPos);
}
