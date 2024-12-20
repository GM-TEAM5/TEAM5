using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusPointReward", menuName = "SO/StageReward/StatusPoint")]
public class StatusPointRewardSO : StageRewardSO 
{
    public int amount;
    
    public override string id => $"000_{amount}";

    public override string dataName => "스탯보상";

    public override void Acquire()
    {
        //
        Player.Instance.status.GetStatusUpgradePoint(amount);
        GamePlayManager.Instance.OpenPlayerStatusUpgradePanel();
    }


}
