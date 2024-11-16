using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAbilitySO : ScriptableObject
{
    public string id;
    public string skillName;
    public Sprite icon;
    //
    [Header("InGameSetting")]
    public float range;
    public int priority;    // 우선순위 : 높을 수록 먼저 시전.
    public float cooltime;      //쿨타임 ( 초 ) 
    public bool uninterruptible;    // 캐스팅을 끊을 수 있는 지. 

    
    //
    public float delay_beforeCast;  // 선딜 : 공격 기술이 
    public float delay_afterCast;   // 후딜

    //
    // public abstract Transform GetTarget(Enemy enemy);ㅔ
    public abstract bool ActivationConditions(Enemy enemy);
    public abstract bool UsageConditions(Enemy enemy);
    public abstract void Use(Enemy enemy);
}
