using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillProperty
{
    Jujak,
    Hyeonmu,
    Cheongryong,
    Backho
}

[CreateAssetMenu(fileName = "SkillReward", menuName = "SO/StageReward/Skill")]
public class SkillRewardSO : StageRewardSO 
{
    public int rank;
    public SkillProperty skillProperty;
    
    public override string id => $"002_{skillProperty}";

    public override string dataName => $"스킬 보상 - {skillProperty}";


    public override void Acquire()
    {
        
    }

}
