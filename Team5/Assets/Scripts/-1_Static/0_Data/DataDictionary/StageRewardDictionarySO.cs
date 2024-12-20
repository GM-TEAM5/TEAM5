using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "StageRewardDictionary", menuName = "SO/Dictionary/StageReward", order = int.MaxValue)]
public class StageRewardDictionarySO : DataDictionarySO
{
    public bool TryGetRandomSkillReward(out SkillRewardSO skillReward)
    {
        skillReward = null;

        List<SkillRewardSO> totalSkillRewards = list.OfType<SkillRewardSO>().OrderBy(x => random.Next()).ToList();

        if (totalSkillRewards.Count>0)
        {
            skillReward = totalSkillRewards[0];
            return true;
        }
        
        return false;
    }

    public bool TryGetStatusPointReward(int amount, out StatusPointRewardSO statusPointReward)
    {
        statusPointReward  = list.OfType<StatusPointRewardSO>().SingleOrDefault(x => x.amount == amount);
        if (statusPointReward != null)
        {
            return true;
        }
        
        return false;
    }
}
