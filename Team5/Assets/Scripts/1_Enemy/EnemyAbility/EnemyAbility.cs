using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAbility
{

    public EnemyAbilitySO data;
    
    public int useCount; 
    public float lastUseTime;   // 스킬 마지막 사용시간 

    public float cooltimeRemain => lastUseTime + data.cooltime - Time.time;
    public bool isCooltimeOk => cooltimeRemain <= 0;

    // Coroutine abilityRoutine;
    Vector3 usingPos;

    public EnemyAbility(EnemyAbilitySO data)
    {
        this.data = data;
        lastUseTime = -data.cooltime + 1 ; // 획득 후 1초 후에 사용되도록.
    }

    //=================================================================================
    public bool CanActiavte(Enemy enemy)
    {
        return data.ActivationConditions(enemy)==true;
    }

    public bool CanUse(Enemy enemy)
    {
        return isCooltimeOk == true;
    }

    public bool CanUseImediatly(Enemy enemy)
    {
        return isCooltimeOk == true && data.UsageConditions(enemy)==true;
    }


    public void ApplyAbility(Vector3 castingPos, Enemy enemy)
    {
        useCount ++;
        data.ApplyAbility( castingPos, enemy );
        lastUseTime = Time.time; //시간기록
    }
}
