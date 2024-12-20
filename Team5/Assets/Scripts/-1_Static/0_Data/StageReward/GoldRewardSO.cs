using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoldReward", menuName = "SO/StageReward/Gold")]
public class GoldRewardSO  : StageRewardSO 
{
    public int amount;
    
    public override string id => $"000_{amount}";

    public override string dataName => $"금화_{amount}";

    public override void Acquire()
    {
        
    }

}
