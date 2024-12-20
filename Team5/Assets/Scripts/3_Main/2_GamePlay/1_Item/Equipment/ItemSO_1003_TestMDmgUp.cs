using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "1003_TestMDmgUp", menuName = "SO/EquipmentItem/1003", order = int.MaxValue)]
public class ItemSO_1003_TestMDmgUp : EquipmentItemSO
{
    public override string id => "1003";
    public override string dataName => "마법 공격력 향상 1";

    public float amount = 5;

    public override void OnEquip()
    {
        Player.Instance.status.ChangeStatus(ref Player.Instance.status.mDmg, amount, true);      
    }
    
    public override void OnUnEquip()
    {
        // 아무것도 안함
        Player.Instance.status.ChangeStatus(ref Player.Instance.status.mDmg, amount, false);      
    }


    protected override void EquipEvent(bool isEquip)
    {
        // GameEventManager.Instance.onChangePlayerStatus_mDmg.Invoke();
    }

}
