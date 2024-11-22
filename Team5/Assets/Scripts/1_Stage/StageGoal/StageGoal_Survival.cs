using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGoal_Survival : StageGoal
{
    public override StageGoalType type => StageGoalType.Survival;
    
    public float targetTime;
    
    public StageGoal_Survival(float targetTime)
    {
        this.targetTime =  targetTime;
    }

    public override bool IsStageClear()
    {
        bool ret=  GamePlayManager.Instance.gamePlayTime >= targetTime && Player.Instance.isAlive;
    
        return ret;
    }
}
