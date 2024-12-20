using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "1009_TestMDmgUp3", menuName = "SO/EquipmentItem/1009", order = int.MaxValue)]
public class ItemSO_1009_TestMDmgUp2 : EquipmentItemSO
{
    public override string id => "1009";
    public override string dataName => "마법 공격력 향상 3";

    public float amount = 777;

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

