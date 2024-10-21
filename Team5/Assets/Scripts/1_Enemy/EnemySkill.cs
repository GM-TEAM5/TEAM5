using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkill 
{    
    public EnemySkillSO skillData;
    
    public int useCount; 
    public float lastUseTime;   // 스킬 마지막 사용시간 

    public float cooltimeRemain => lastUseTime + skillData.cooltime - Time.time;
    public bool isCooltimeOk => cooltimeRemain <= 0;

    public EnemySkill(EnemySkillSO skillData)
    {
        this.skillData = skillData;
        lastUseTime = -skillData.cooltime + 1 ; // 획득 후 1초 후에 사용되도록.
    }


    public void Use(Enemy enemy, Vector3 targetPos)
    {
        useCount ++;
        lastUseTime = Time.time; //시간기록
        
        //        
        skillData.Use( enemy, targetPos );
    }
}
