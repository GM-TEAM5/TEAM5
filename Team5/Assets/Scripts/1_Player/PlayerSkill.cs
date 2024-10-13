using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill 
{
    public PlayerSkillSO skillData;
    
    public int useCount; 
    public float lastUseTime;   // 스킬 마지막 사용시간 

    public float cooltimeRemain => lastUseTime + skillData.cooltime - Time.time;
    public bool isAvailable => cooltimeRemain <= 0;

    public PlayerSkill(PlayerSkillSO skillData)
    {
        this.skillData = skillData;
        lastUseTime = -skillData.cooltime + 1 ; // 획득 후 1초 후에 사용되도록.
    }


    public void Use()
    {
        useCount ++;
        lastUseTime = Time.time; //시간기록
        
        //
        Vector3 targetPos = skillData.FindTargetPos();
        
        skillData.Use( targetPos );
    }

}
