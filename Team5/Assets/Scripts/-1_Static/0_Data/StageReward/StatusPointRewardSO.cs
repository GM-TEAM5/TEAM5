using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusPointReward", menuName = "SO/StageReward/StatusPoint")]
public class StatusPointRewardSO : StageRewardSO 
{
    public int amount;
    
    public override string id => $"001_{amount}";

    public override string dataName => $"스탯보상_{amount}";

    public override void Acquire()
    {
        //
        Player.Instance.status.GetStatusUpgradePoint(amount);
        GamePlayManager.Instance.OpenPlayerStatusUpgradePanel();
    }


}
