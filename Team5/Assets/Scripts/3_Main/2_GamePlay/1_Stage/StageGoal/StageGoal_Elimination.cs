using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageGoal_Elimination : StageGoal
{
    public override StageGoalType type => StageGoalType.Elimination;
    
    public SerializableDictionary<string,int> targetCount;
    public SerializableDictionary<int, SerializableDictionary<string,int>> waveTargetCount;
    
    public StageGoal_Elimination(SerializableDictionary<int,List<WaveInfo>> waveInfos )
    {
       

        foreach(var kv in waveInfos)
        {
            
        }
        
    }

    public override bool IsStageClear()
    {
        bool ret=  targetCount.Values.Sum() == 0;
    
        return ret;
    }
}